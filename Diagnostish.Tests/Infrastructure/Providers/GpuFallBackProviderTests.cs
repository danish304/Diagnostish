using Diagnostish.Domain.Common;
using Diagnostish.Infrastructure.Providers;
using Diagnostish.Infrastructure.Providers.Common.RawModels.Hardware;
using Diagnostish.Infrastructure.Providers.Registry.Common;
using Diagnostish.Infrastructure.Providers.Wmi.Common;

namespace Diagnostish.Tests.Infrastructure.Providers;

public class GpuFallBackProviderTests
{
    private const double UINT32_OVERFLOW = 4_290_000_000;

    private readonly IWmiSource<GpuRawModel> _wmiProvider =
        Substitute.For<IWmiSource<GpuRawModel>>();

    private readonly IRegistrySource<GpuRawModel> _registryProvider =
        Substitute.For<IRegistrySource<GpuRawModel>>();

    private readonly GpuFallBackProvider _sut;

    public GpuFallBackProviderTests()
    {
        _sut = new GpuFallBackProvider(_wmiProvider, _registryProvider);
    }

    public static TheoryData<long?> InvalidAdapterRamValues() => new()
    {
        null,
        0L,
        -100L,
        (long)UINT32_OVERFLOW,
        (long)UINT32_OVERFLOW + 1000
    };

    [Theory]
    [MemberData(nameof(InvalidAdapterRamValues))]
    public async Task ProvideInfoAsync_WmiReturnsInvalidValues(long? invalidAdapterRam)
    {
        // Arrange
        var wmiGpu = new GpuRawModel(
            "NVIDIA GeForce RTX 4060 Ti",
            invalidAdapterRam
        );

        _wmiProvider.ProvideAsync(Arg.Any<CancellationToken>())
            .Returns(ProvideResult<IReadOnlyList<GpuRawModel>>.Ok(
                [wmiGpu],
                warnings: ["WMI: некорректные данные"])
            );

        var registryGpu = new GpuRawModel(
            "NVIDIA GeForce RTX 4060 Ti",
            8_585_740_288
        );

        _registryProvider.ProvideAsync(Arg.Any<CancellationToken>())
            .Returns(ProvideResult<IReadOnlyList<GpuRawModel>>.Ok([registryGpu]));

        // Act
        var result = await _sut.ProvideAsync();

        // Assert
        result.Data!.Should().Contain(registryGpu);
        result.Warnings.Should().Contain(
            "WMI: некорректные данные",
            "Объём видеопамяти для части адаптеров получен из реестра — WMI вернул некорректное значение."
        );

        result.CriticalErrors.Should().BeEmpty();
    }

    [Theory]
    [MemberData(nameof(InvalidAdapterRamValues))]
    public async Task ProvideInfoAsync_WmiReturnsInvalidValuesButRegistryFails(long? invalidAdapterRam)
    {
        // Arrange
        var wmiGpu = new GpuRawModel(
            "NVIDIA GeForce RTX 4060 Ti",
            invalidAdapterRam
        );

        _wmiProvider.ProvideAsync(Arg.Any<CancellationToken>())
            .Returns(ProvideResult<IReadOnlyList<GpuRawModel>>.Ok([wmiGpu]));

        _registryProvider.ProvideAsync(Arg.Any<CancellationToken>())
            .Returns(ProvideResult<IReadOnlyList<GpuRawModel>>.Fail(
                warnings: [],
                criticalErrors: ["Реестр: нет доступа"])
            );

        // Act
        var result = await _sut.ProvideAsync();

        // Assert
        result.Data!.Should().Contain(wmiGpu);
        result.Warnings.Should().Contain("Реестр: нет доступа");
        result.CriticalErrors.Should().BeEmpty();
    }

    [Fact]
    public async Task ProvideInfoAsync_WmiReturnsValidValues()
    {
        // Arrange
        var wmiGpu = new GpuRawModel(
            "NVIDIA GeForce RTX 4060 Ti",
            1_073_741_824
        );

        _wmiProvider.ProvideAsync(Arg.Any<CancellationToken>())
            .Returns(ProvideResult<IReadOnlyList<GpuRawModel>>.Ok([wmiGpu]));

        // Act
        var result = await _sut.ProvideAsync();

        // Assert
        result.Data!.Should().Contain(wmiGpu);
        result.Warnings.Should().BeEmpty();
        result.CriticalErrors.Should().BeEmpty();

        await _registryProvider.DidNotReceive().
            ProvideAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ProvideInfoAsync_RegistryAndWmiFails()
    {
        // Arrange
        _wmiProvider.ProvideAsync(Arg.Any<CancellationToken>())
            .Returns(ProvideResult<IReadOnlyList<GpuRawModel>>.Fail(
                warnings: [],
                criticalErrors: ["WMI: нет доступа"])
            );

        _registryProvider.ProvideAsync(Arg.Any<CancellationToken>())
            .Returns(ProvideResult<IReadOnlyList<GpuRawModel>>.Fail(
                warnings: [],
                criticalErrors: ["Реестр: нет доступа"])
            );

        // Act
        var result = await _sut.ProvideAsync();

        // Assert
        result.Data.Should().BeNull();
        result.Warnings.Should().BeEmpty();
        result.CriticalErrors.Should().Contain(
            "WMI: нет доступа",
            "Реестр: нет доступа"
        );
    }

    [Fact]
    public async Task ProvideInfoAsync_KeepsWmiValuesIfRegistryHasNoMatchingNames()
    {
        // Arrange
        var wmiGpu = new GpuRawModel(null, 4_293_918_720);

        _wmiProvider.ProvideAsync(Arg.Any<CancellationToken>())
            .Returns(ProvideResult<IReadOnlyList<GpuRawModel>>.Ok([wmiGpu]));

        var registryGpu = new GpuRawModel(
            "NVIDIA GeForce RTX 4060 Ti",
            8_585_740_288
        );

        _registryProvider.ProvideAsync(Arg.Any<CancellationToken>())
            .Returns(ProvideResult<IReadOnlyList<GpuRawModel>>.Ok([registryGpu]));

        // Act
        var result = await _sut.ProvideAsync();

        // Assert
        result.Data!.Should().Contain(wmiGpu);
        result.Warnings.Should()
            .Contain("Не удалось определить точный объём видеопамяти для одного из адаптеров ни через WMI, ни через реестр.");

        result.CriticalErrors.Should().BeEmpty();
    }

    [Fact]
    public async Task ProvideInfoAsync_ReplacesOnlyInvalidGpusWhenMultipleAdaptersReturned()
    {
        // Arrange
        var validGpu = new GpuRawModel(
            "Intel UHD Graphics",
            1_073_741_824
        );

        var invalidGpu = new GpuRawModel(
            "NVIDIA GeForce RTX 4060 Ti",
            4_293_918_720
        );
        _wmiProvider.ProvideAsync(Arg.Any<CancellationToken>())
            .Returns(ProvideResult<IReadOnlyList<GpuRawModel>>.Ok(
                [validGpu, invalidGpu],
                warnings: ["WMI: некорректные данные"])
            );

        var registryGpu = new GpuRawModel(
            "NVIDIA GeForce RTX 4060 Ti",
            8_585_740_288
        );

        _registryProvider.ProvideAsync(Arg.Any<CancellationToken>())
            .Returns(ProvideResult<IReadOnlyList<GpuRawModel>>.Ok([registryGpu]));

        // Act
        var result = await _sut.ProvideAsync();

        // Assert
        result.Data.Should().HaveCount(2);
        result.Data.Should().Contain(validGpu);
        result.Data.Should().Contain(registryGpu);
        result.Warnings.Should().Contain(
            "WMI: некорректные данные",
            "Объём видеопамяти для части адаптеров получен из реестра — WMI вернул некорректное значение."
        );

        result.CriticalErrors.Should().BeEmpty();
    }

    [Fact]
    public async Task ProvideInfoAsync_ReplacesAllGpusWhenAllAreInvalid()
    {
        // Arrange
        var invalidGpu1 = new GpuRawModel(
            "NVIDIA GeForce RTX 4060 Ti",
            4_293_918_720
        );

        var invalidGpu2 = new GpuRawModel("Intel Arc A770", -100L);
        var invalidGpu3 = new GpuRawModel("AMD Radeon RX 7600", null);

        _wmiProvider.ProvideAsync(Arg.Any<CancellationToken>())
            .Returns(ProvideResult<IReadOnlyList<GpuRawModel>>.Ok(
                [invalidGpu1, invalidGpu2, invalidGpu3],
                warnings: ["WMI: некорректные данные"])
            );

        var registryGpu1 = new GpuRawModel(
            "NVIDIA GeForce RTX 4060 Ti",
            8_585_740_288
        );

        var registryGpu2 = new GpuRawModel(
            "Intel Arc A770",
            17_179_869_184
        );

        var registryGpu3 = new GpuRawModel(
            "AMD Radeon RX 7600",
            2_147_483_648
        );

        _registryProvider.ProvideAsync(Arg.Any<CancellationToken>())
            .Returns(ProvideResult<IReadOnlyList<GpuRawModel>>.Ok([registryGpu1, registryGpu2, registryGpu3]));

        // Act
        var result = await _sut.ProvideAsync();

        // Assert
        result.Data.Should().HaveCount(3);
        result.Data.Should().Contain(registryGpu1);
        result.Data.Should().Contain(registryGpu2);
        result.Data.Should().Contain(registryGpu3);
        result.Warnings.Should().Contain(
            "WMI: некорректные данные",
            "Объём видеопамяти для части адаптеров получен из реестра — WMI вернул некорректное значение."
        );

        result.CriticalErrors.Should().BeEmpty();
    }

    [Fact]
    public async Task ProvideInfoAsync_PassesCancellationTokenToBothProviders()
    {
        // Arrange
        using var cts = new CancellationTokenSource();

        var wmiGpu = new GpuRawModel(
            "NVIDIA GeForce RTX 4060 Ti",
            4_293_918_720
        );

        _wmiProvider.ProvideAsync(Arg.Any<CancellationToken>())
            .Returns(ProvideResult<IReadOnlyList<GpuRawModel>>.Ok([wmiGpu]));

        var registryGpu = new GpuRawModel(
            "NVIDIA GeForce RTX 4060 Ti",
            8_585_740_288
        );

        _registryProvider.ProvideAsync(Arg.Any<CancellationToken>())
            .Returns(ProvideResult<IReadOnlyList<GpuRawModel>>.Ok([registryGpu]));

        // Act
        await _sut.ProvideAsync(cts.Token);

        // Assert
        await _wmiProvider.Received(1).ProvideAsync(cts.Token);
        await _registryProvider.Received(1).ProvideAsync(cts.Token);
    }
}