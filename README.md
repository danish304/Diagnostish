<div align="center">
 
# Diagnostish 3.1 #

<img src="https://raw.githubusercontent.com/danish304/Diagnostish/refs/heads/master/Diagnostish.Desktop/AppIcon.ico" width="100" height="100" alt="Логотип">

</div>

<div>
<br>
</div>

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Портативная консольная утилита на `C#` для автоматизации первичной диагностики и сбора технических/программных характеристик компьютера. Разработана специально для запуска с сервисных флеш-накопителей.

<div>
<br>
</div>

> [!IMPORTANT]
> ## ⚖️ Лицензия ##
> Этот проект распространяется под лицензией *[`MIT`](LICENSE.txt)*. Вы можете свободно использовать, изменять и распространять код.

> [!WARNING]
> ## 📋 Требования ##
> * ***ОС:*** Windows 7 SP1 и выше.
> * ***Права:*** Администратор (требуется для доступа к WMI-сенсорам; манифест приложения запрашивает повышение автоматически).
> * ***.NET Runtime:*** Встроен (self-contained билд).

## ✨ Основной функционал ##

* ***Сбор аппаратных данных.*** Низкоуровневый асинхронный опрос компонентов (процессор, оперативная память, видеокарты, накопители, материнская плата, BIOS) и данных операционной системы через *`Windows Management Instrumentation (WMI)`* и реестр.
* ***Многоуровневая отказоустойчивость.*** Безопасный парсинг данных ([`Parser`](Diagnostish.Infrastructure/Shared/Utils/Parser.cs)) и безопасное чтение реестра([`ExecutorRegistry`](Diagnostish.Infrastructure/Shared/Registry/Executor/ExecutorRegistry.cs))/выполнение WMI-запросов ([`ExecutorWmi`](Diagnostish.Infrastructure/Shared/Wmi/Executor/ExecutorWmi.cs)) с раздельной обработкой ошибок доступа, таймаутов и повреждённых записей. Отсутствие или некорректность отдельных полей не прерывает сбор — утилита продолжает работу, фиксируя предупреждения (*`Warnings`*) и критические ошибки (*`CriticalErrors`*) в итоговом отчёте.
* ***Автоматизация прав доступа.*** Встроенный манифест приложения (*`UAC`*) запрашивает права администратора автоматически, что необходимо для корректного чтения системных WMI-датчиков.
* ***Автономность.*** Утилита собирается в один исполняемый файл со всеми зависимостями. Работает на любом ПК с *`Windows`* без предварительной установки *`.NET`*.
* ***Информативность.*** Результаты сканирования структурированы и выводятся в кастомном цветовом интерфейсе консоли.
* ***Логирование.*** Все события и ошибки записываются в директорию *`/logs`* в формате текстового журнала (*`Serilog`*). В случае сбоя лог-файл помогает быстро локализовать проблему.
* ***Конфигурирование.*** После запуска приложения создается файл с настройками *`appsettings.json`*, пользователь может при необходимости изменять настройки работы утилиты.

## 🛠️ Архитектура и технологии ##

* Проект спроектирован по принципам *`Clean Architecture (Domain → Application/Infrastructure → Desktop)`* с соблюдением *`SOLID`*, что обеспечивает независимость бизнес-логики от конкретных технологий сбора и вывода данных, а также лёгкость расширения и тестирования.
* ***Технологический стек:*** *`C# (.NET 10)`*, *`Serilog`*, *`System.Management (WMI) + Microsoft.Win32 (Реестр)`*, *`Microsoft.Extensions.DependencyInjection`*, *`xUnit + FluentAssertions + NSubstitute`*.

<div>
<br>
</div>

> [!TIP]
> ## 📦 Как использовать ##
> * Скачайте последний релиз утилиты во вкладке `Releases`.
> * Поместите скомпилированный `.exe` на сервисную флешку.
> * Если требуется, создайте и настройте файл конфигурации *`appsettings.json`* в папке с исполняемым файлом, или измените в нем параметры после первого запуска приложения (*`.json`* создастся автоматически).
> * Запустите утилиту на целевом ПК — она автоматически запросит права администратора, соберёт диагностические данные и выведет их в консольном интерфейсе.

<div>
<br>
</div>

## 📂 Структура ##
### Diagnostish.Domain ###

**Ядро приложения — не зависит ни от одного другого проекта решения.**
  
* [`Entities`](Diagnostish.Domain/Models/Entities/) — доменные сущности с провалидированными данными (*`CpuInfo`*, *`RamInfo`*, *`GpuInfo`*, *`StorageDriveInfo`*, *`BiosInfo`*, *`BaseBoardInfo`*, *`OperatingSystemInfo`*).
* [`Reports`](Diagnostish.Domain/Models/Reports/) — плоские модели для представления результатов (*`HardwareReport`*, *`OperatingSystemReport`*, объединяющий их *`FinalReport`*), а также базовый *`IssuesReport`* со списками *`Warnings/CriticalErrors`*.
* [`ProvideResult`](Diagnostish.Domain/Common/ProvideResult.cs) — единый «конверт» результата на каждом этапе конвейера: данные (или *`null`* при полном сбое) плюс списки предупреждений и критических ошибок.
* [`Interfaces`](Diagnostish.Domain/Interfaces/) — контракты, не привязанные ни к *`WMI/Реестру`*, ни к консоли:
  * [`IProvideDiagnosticInfo`](Diagnostish.Domain/Interfaces/IProvideDiagnosticInfo.cs) — сбор сырых данных;
  * [`IAnalyzeDiagnosticInfo`](Diagnostish.Domain/Interfaces/IAnalyzeDiagnosticInfo.cs) — анализ и валидация;
  * [`IReportMapper`](Diagnostish.Domain/Interfaces/IReportMapper.cs) — перенос провалидированных данных в отчёт;
  * [`IReportPrinter`](Diagnostish.Domain/Interfaces/IReportPrinter.cs) — вывод готового отчёта;
  * [`IUserInterface`](Diagnostish.Domain/Interfaces/IUserInterface.cs) — взаимодействие с пользователем (приветствие, ожидание выхода).

### Diagnostish.Infrastructure ###

**Реализация сбора данных через *`WMI`* или через реестр при ошибках (FallBack). Зависит только от [`Domain`](Diagnostish.Domain/Diagnostish.Domain.csproj).**

* [`BaseWmiProvider`](Diagnostish.Infrastructure/Providers/Common/BaseWmiProvider.cs) и [`BaseRegistryProvider`](Diagnostish.Infrastructure/Providers/Common/BaseRegistryProvider.cs) — абстрактные базовые классы (*`Template Methods`*), инкапсулирующие общую механику WMI-запроса/чтения реестра. Реализуют узкие интерфейсы [`IWmiSource`](Diagnostish.Infrastructure/Providers/Common/IWmiSource.cs) и [`IRegistrySource`](Diagnostish.Infrastructure/Providers/Common/IRegistrySource.cs).
* [`HardwareInfoProviders`](Diagnostish.Infrastructure/Providers/HardwareInfoProviders/), [`OperatingSystemProviders`](Diagnostish.Infrastructure/Providers/OperatingSystemInfoProviders/) — конкретные провайдеры (*`CpuInfoWmiProvider`*, *`GpuInfoRegistryProvider`* и т.д.) и их «сырые» *`DTO`* (*`RawCpuInfo`*, *`RawRamInfo`* и т.д.) в подпапке [`RawHardwareInfo`](Diagnostish.Infrastructure/Providers/HardwareInfoProviders/RawHardwareInfo/).
* [`Analyzers`](Diagnostish.Infrastructure/Analyzers/) — бизнес-логика валидации: превращение сырых данных в доменные сущности с одновременной генерацией предупреждений. Общие проверки вынесены в [`AnalyzerValidationExtensions`](Diagnostish.Infrastructure/Analyzers/Common/AnalyzerValidationExtensions.cs) (*`GetValueOrWarning`* для строк, чисел через *`INumber<T>`* и дат). Тексты предупреждений сгруппированы по компоненту в [`Messages`](Diagnostish.Infrastructure/Analyzers/HardwareInfoAnalyzers/Messages/).
* [`Wmi`](Diagnostish.Infrastructure/Shared/Wmi/) — [`ExecutorWmi`](Diagnostish.Infrastructure/Shared/Wmi/Executor/ExecutorWmi.cs)/[`IExecutorWmi`](Diagnostish.Infrastructure/Shared/Wmi/Executor/IExecutorWmi.cs) (безопасное выполнение запросов с раздельной обработкой *`OperationCanceledException`*, *`ManagementException.Timedout`*), [`WmiExecutorMessages`](Diagnostish.Infrastructure/Shared/Wmi/Executor/WmiExecutorMessages.cs), [`WmiSettings`](Diagnostish.Infrastructure/Shared/Wmi/WmiSettings.cs).
* [`Registry`](Diagnostish.Infrastructure/Shared/Registry/) — [`ExecutorRegistry`](Diagnostish.Infrastructure/Shared/Registry/Executor/ExecutorRegistry.cs)/[`IExecutorRegistry`](Diagnostish.Infrastructure/Shared/Registry/Executor/IExecutorRegistry.cs) (безопасное чтение реестра с обработкой *`OperationCanceledException`*), [`RegistryExecutorMessages`](Diagnostish.Infrastructure/Shared/Registry/Executor/RegistryExecutorMessages.cs).
* [`Utils`](Diagnostish.Infrastructure/Shared/Utils/) — [`Parser`](Diagnostish.Infrastructure/Shared/Utils/Parser.cs) (безопасное приведение WMI-значений к nullable-типам **C#**) и [`ByteConverter`](Diagnostish.Infrastructure/Shared/Utils/ByteConverter.cs) (перевод байт в гигабайты).

### Diagnostish.Application ###

**Оркестрация бизнес-процесса. Зависит только от [`Domain`](Diagnostish.Domain/Diagnostish.Domain.csproj).**

* [`ComponentPipeline`](Diagnostish.Application/Pipelines/ComponentPipeline.cs) — типизированная обёртка над *`Action<TReport>`*, представляющая полный цикл «собрать → проанализировать → замапить» для одного компонента.
* [`Mappers`](Diagnostish.Application/Mappers/) — реализации [`IReportMapper`](Diagnostish.Domain/Interfaces/IReportMapper.cs) для каждого компонента; общая логика (перенос *`Warnings/CriticalErrors`*, безопасное извлечение данных) вынесена в [`MapperExtensions`](Diagnostish.Application/Mappers/Common/MapperExtensions.cs).
* [`ServicesAggregator`](Diagnostish.Application/Services/ServicesAggregator.cs) — собирает [`FinalReport`](Diagnostish.Domain/Models/Reports/FinalReport.cs), прогоняя все зарегистрированные [`ComponentPipeline`](Diagnostish.Application/Pipelines/ComponentPipeline.cs) для аппаратной части и части ОС.

### Diagnostish.Desktop ###

***`Composition Root`* и точка входа. Единственный проект, которому разрешено знать одновременно про [`Infrastructure`](Diagnostish.Infrastructure/Diagnostish.Infrastructure.csproj) и конкретные реализации представления.**

* [`ServiceCollectionExtensions`](Diagnostish.Desktop/Composition/ServiceCollectionExtensions.cs) — универсальные приватные generic-методы регистрации: *`AddComponent<TReport, TRaw, TInfo, TProvider, TAnalyzer, TMapper>()`* регистрирует провайдер, анализатор, маппер и собирает из них [`ComponentPipeline`](Diagnostish.Application/Pipelines/ComponentPipeline.cs) одной строкой; *`AddPrinter<TReport, TPrinter>()`* регистрирует принтер отчёта: нужны для методов расширения, которые добавляют коллекции зависимостей.
* [`LoggerConfigurator`](Diagnostish.Desktop/Composition/LoggerConfigurator.cs) — настройка *`Serilog`* (запись в *`/logs`* с ротацией по дням).
* [`ReportPrinter`](Diagnostish.Desktop/Views/Common/ReportPrinter.cs) — базовый класс принтера (*`Template Method: PrintReport`* + общий вывод *`Warnings/CriticalErrors`*).
* [`HardwareInfoPrinters`](Diagnostish.Desktop/Views/HardwareInfoPrinters/), [`OperatingSystemInfoPrinters`](Diagnostish.Desktop/Views/OperatingSystemInfoPrinters/) — консольные принтеры отчётов.
* [`PrintersAggregator`](Diagnostish.Desktop/Views/PrintersAggregator.cs) — рассылает готовый [`FinalReport`](Diagnostish.Domain/Models/Reports/FinalReport.cs) по всем зарегистрированным принтерам (позволяет добавлять новые способы вывода — например, в файл — не меняя контроллер).
* [`DiagnosticController`](Diagnostish.Desktop/Controllers/DiagnosticController.cs) — тонкий оркестратор: приветствие → сбор отчёта → вывод во все принтеры → ожидание выхода.
* [`Program.cs`](Diagnostish.Desktop/Program.cs) — точка сборки: настройка DI-контейнера, регистрация всех компонентов и принтеров, асинхронный запуск контроллера.

### Diagnostish.Tests ###

**Юнит-тесты (*`xUnit + FluentAssertions1`*) для [`Infrastructure`](Diagnostish.Infrastructure/Diagnostish.Infrastructure.csproj) — на данный момент покрывают [`Parser`](Diagnostish.Infrastructure/Shared/Utils/Parser.cs) (безопасное приведение типов, включая некорректные CIM-даты) и [`GpuInfoFallBackProvider`](Diagnostish.Infrastructure/Providers/HardwareInfoProviders/GpuInfoFallBackProvider.cs) (проверка выполнения FallBack, пограничные случаи, проверка передачи токена).**

<div>
<br>
</div>

> [!NOTE]
> ## 🔭 Планы развития ##
> * Расширение набора принтеров (вывод в файл).
> * Сбор и вывод информации о сети.
