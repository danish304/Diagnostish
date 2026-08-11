<div align="center">
 
# Diagnostish 3.3 #

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
> * ***Права:*** Администратор (требуется для доступа к WMI-сенсорам и разделам реестра; манифест приложения запрашивает повышение автоматически).
> * ***.NET Runtime:*** Встроен (self-contained билд).

## ✨ Основной функционал ##

* ***Сбор аппаратных данных.*** Низкоуровневый **асинхронный, параллельный** опрос компонентов (процессор, оперативная память, видеокарты, накопители, материнская плата, BIOS) и данных операционной системы через *`Windows Management Instrumentation (WMI)`*, с возможностью подключения альтернативных источников (реестр *`Windows`*) для отдельных компонентов.
* ***Резервный сбор данных о видеопамяти.*** Для видеокарт, у которых *`WMI`* (`Win32_VideoController.AdapterRAM`) возвращает некорректный объём из-за ограничения *`uint32`* (переполнение около 4 ГБ), точное значение автоматически подставляется из реестра.
* ***Многоуровневая отказоустойчивость.*** Безопасный парсинг данных ([`Parser`](Diagnostish.Infrastructure/Shared/Common/Utils/Parser.cs)) и безопасное асинхронное выполнение запросов ([`WmiExecutor`](Diagnostish.Infrastructure/Shared/Wmi/Executor/WmiExecutor.cs), [`RegistryExecutor`](Diagnostish.Infrastructure/Shared/Registry/Executor/RegistryExecutor.cs)) с раздельной обработкой ошибок доступа, таймаутов, отмены и повреждённых записей. Отсутствие или некорректность отдельных полей не прерывает сбор — утилита продолжает работу, фиксируя предупреждения (*`Warnings`*) и критические ошибки (*`CriticalErrors`*) в итоговом отчёте.
* ***Параллельный сбор.*** Все компоненты опрашиваются одновременно, а не последовательно — общее время диагностики ограничено самым медленным запросом, а не их суммой.
* ***Корректная отмена.*** Сканирование можно прервать сочетанием `Ctrl+C` — приложение перехватывает сигнал, останавливает текущие запросы и завершает работу штатно, с записью в лог, без аварийного обрыва процесса.
* ***Автоматизация прав доступа.*** Встроенный манифест приложения (*`UAC`*) запрашивает права администратора автоматически.
* ***Автономность.*** Утилита собирается в один исполняемый файл со всеми зависимостями. Работает на любом ПК с *`Windows`* без предварительной установки *`.NET`*.
* ***Информативность.*** Результаты сканирования структурированы и выводятся в кастомном цветовом интерфейсе консоли.
* ***Логирование.*** Все события и ошибки записываются в директорию *`/logs`* в формате текстового журнала (*`Serilog`*).
* ***Конфигурирование.*** При первом запуске рядом с исполняемым файлом автоматически создаётся *`appsettings.json`* со значениями по умолчанию (например, таймаут WMI-запросов). При его отсутствии или повреждении утилита прозрачно откатывается на встроенные значения по умолчанию.

## 🛠️ Архитектура и технологии ##

* Проект спроектирован по принципам *`Clean Architecture (Domain → Application/Infrastructure → Desktop)`* с соблюдением *`SOLID`*.
* ***Технологический стек:*** *`C# (.NET 10)`*, *`Serilog`*, *`System.Management (WMI)`*, *`Microsoft.Win32.Registry`*, *`Microsoft.Extensions.DependencyInjection`*, *`Microsoft.Extensions.Configuration/Options`*, *`xUnit + FluentAssertions + NSubstitute`*.

<div>
<br>
</div>

> [!TIP]
> ## 📦 Как использовать ##
> * Скачайте последний релиз утилиты во вкладке `Releases`.
> * Поместите скомпилированный `.exe` на сервисную флешку.
> * Запустите утилиту на целевом ПК — она автоматически запросит права администратора, соберёт диагностические данные и выведет их в консольном интерфейсе. Файл конфигурации *`appsettings.json`* будет создан автоматически при первом запуске.
> * Сканирование можно в любой момент остановить сочетанием `Ctrl+C`.

<div>
<br>
</div>

## 📂 Структура ##

###  Diagnostish.Domain ###

**Ядро приложения — не зависит ни от одного другого проекта решения.**

* [`Models/Entities`](Diagnostish.Domain/Models/Entities/) — доменные сущности с провалидированными данными: [`Hardware`](Diagnostish.Domain/Models/Entities/Hardware/) (*`Cpu`*, *`Ram`*, *`Gpu`*, *`StorageDrive`*, *`Bios`*, *`BaseBoard`*) и [`OperatingSystem`](Diagnostish.Domain/Models/Entities/OperatingSystem/).
* [`Models/Reports`](Diagnostish.Domain/Models/Reports/) — плоские модели результатов: [`HardwareReport`, `OperatingSystemReport`](Diagnostish.Domain/Models/Reports/Components/), объединяющий их [`FinalReport`](Diagnostish.Domain/Models/Reports/FinalReport.cs), а также базовый [`BaseIssuesReport`](Diagnostish.Domain/Models/Reports/Common/BaseIssuesReport.cs) со списками *`Warnings/CriticalErrors`*.
* [`ProvideResult`](Diagnostish.Domain/Common/ProvideResult.cs) — единый «конверт» результата на каждом этапе конвейера: данные (или *`null`* при полном сбое) плюс списки предупреждений и критических ошибок.
* [`Interfaces`](Diagnostish.Domain/Interfaces/) — контракты, не привязанные ни к *`WMI`*/реестру, ни к консоли:
  * [`IProvider`](Diagnostish.Domain/Interfaces/IProvider.cs) — асинхронный сбор сырых данных, не зависящий от источника (WMI, реестр или иной);
  * [`IAnalyzer`](Diagnostish.Domain/Interfaces/IAnalyzer.cs) — синхронный анализ и валидация;
  * [`IReportMapper`](Diagnostish.Domain/Interfaces/IReportMapper.cs) — перенос провалидированных данных в отчёт;
  * [`IReportPrinter`](Diagnostish.Domain/Interfaces/IReportPrinter.cs) — вывод готового отчёта;
  * [`IUserInterface`](Diagnostish.Domain/Interfaces/IUserInterface.cs) — взаимодействие с пользователем.

### Diagnostish.Infrastructure ###

**Реализация сбора данных через *`WMI`* и реестр *`Windows`*. Зависит только от [`Domain`](Diagnostish.Domain/Diagnostish.Domain.csproj).**

* [`Providers/Common/RawModels`](Diagnostish.Infrastructure/Providers/Common/RawModels/) — «сырые» *`DTO`*, общие для всех источников одного компонента.
* [`Providers/Wmi`](Diagnostish.Infrastructure/Providers/Wmi/) — [`BaseWmiProvider`](Diagnostish.Infrastructure/Providers/Wmi/Common/BaseWmiProvider.cs) (*`Template Method`*, общая механика WMI-запроса) и конкретные провайдеры в [`Hardware`](Diagnostish.Infrastructure/Providers/Wmi/Hardware/) / [`OperatingSystem`](Diagnostish.Infrastructure/Providers/Wmi/OperatingSystem/).
* [`Providers/Registry`](Diagnostish.Infrastructure/Providers/Registry/) — [`BaseRegistryProvider`](Diagnostish.Infrastructure/Providers/Registry/Common/BaseRegistryProvider.cs) (тот же паттерн для источников на базе реестра) и [`GpuRegistryProvider`](Diagnostish.Infrastructure/Providers/Registry/GpuRegistryProvider.cs).
* [`GpuFallBackProvider`](Diagnostish.Infrastructure/Providers/GpuFallBackProvider.cs) — комбинирует *`GpuWmiProvider`* и *`GpuRegistryProvider`*: при переполнении/некорректности объёма видеопамяти в WMI подставляет значение из реестра, сохраняя прозрачность через *`Warnings`*.
* [`Analyzers`](Diagnostish.Infrastructure/Analyzers/) — синхронная бизнес-логика валидации, разнесённая по [`Hardware`](Diagnostish.Infrastructure/Analyzers/Hardware/) и [`OperatingSystem`](Diagnostish.Infrastructure/Analyzers/OperatingSystem/). Общие проверки — в [`AnalyzerValidationExtensions`](Diagnostish.Infrastructure/Analyzers/Common/AnalyzerValidationExtensions.cs) (*`GetValueOrWarning`* для строк, чисел через *`INumber<T>`* и дат). Тексты предупреждений — в подпапках *`Messages`* каждого раздела.
* [`Shared/Wmi`](Diagnostish.Infrastructure/Shared/Wmi/) — [`WmiExecutor`](Diagnostish.Infrastructure/Shared/Wmi/Executor/WmiExecutor.cs) (асинхронное выполнение запросов с раздельной обработкой ошибок доступа, таймаута и отмены), настраиваемый [`WmiSettings`](Diagnostish.Infrastructure/Shared/Wmi/WmiSettings.cs).
* [`Shared/Registry`](Diagnostish.Infrastructure/Shared/Registry/) — [`RegistryExecutor`](Diagnostish.Infrastructure/Shared/Registry/Executor/RegistryExecutor.cs) — тот же принцип безопасного асинхронного выполнения, применённый к чтению реестра.
* [`Shared/Common/Utils`](Diagnostish.Infrastructure/Shared/Common/Utils/) — [`Parser`](Diagnostish.Infrastructure/Shared/Common/Utils/Parser.cs) (безопасное приведение значений произвольного источника к nullable-типам **C#**) и [`ByteConverter`](Diagnostish.Infrastructure/Shared/Common/Utils/ByteConverter.cs) (перевод байт в гигабайты).

### Diagnostish.Application ###

**Оркестрация бизнес-процесса. Зависит только от [`Domain`](Diagnostish.Domain/Diagnostish.Domain.csproj).**

* [`ComponentPipeline`](Diagnostish.Application/Pipelines/ComponentPipeline.cs) — типизированная обёртка над `Func<CancellationToken, Task<Action<TReport>>>`, разделяющая цикл компонента на асинхронный сбор+анализ (может выполняться параллельно с другими компонентами) и последующую синхронную запись в отчёт (строго последовательно — исключает гонки данных при конкурентном доступе к *`Warnings`*/*`CriticalErrors`*).
* [`Mappers`](Diagnostish.Application/Mappers/) — реализации [`IReportMapper`](Diagnostish.Domain/Interfaces/IReportMapper.cs) по [`Hardware`](Diagnostish.Application/Mappers/Hardware/) и [`OperatingSystem`](Diagnostish.Application/Mappers/OperatingSystem/); общая логика — в [`MapperExtensions`](Diagnostish.Application/Mappers/Common/MapperExtensions.cs).
* [`FinalReportComposer`](Diagnostish.Application/Services/FinalReportComposer.cs) — асинхронно собирает [`FinalReport`](Diagnostish.Domain/Models/Reports/FinalReport.cs): параллельно запускает сбор и анализ всех зарегистрированных [`ComponentPipeline`](Diagnostish.Application/Pipelines/ComponentPipeline.cs), затем последовательно применяет результаты к отчётам.

### Diagnostish.Desktop ###

***`Composition Root`* и точка входа. Единственный проект, которому разрешено знать одновременно про [`Infrastructure`](Diagnostish.Infrastructure/Diagnostish.Infrastructure.csproj) и конкретные реализации представления.**

* [`ServiceCollectionExtensions`](Diagnostish.Desktop/Composition/ServiceCollectionExtensions.cs) — generic-регистрация: `AddComponent<TReport, TRaw, TData, TProvider, TAnalyzer, TMapper>()` регистрирует провайдер (WMI-, реестровый или композитный), анализатор, маппер и собирает [`ComponentPipeline`](Diagnostish.Application/Pipelines/ComponentPipeline.cs) одной строкой; `AddPrinter<TReport, TPrinter>()` регистрирует принтер.
* [`LoggerConfigurator`](Diagnostish.Desktop/Composition/LoggerConfigurator.cs) — настройка *`Serilog`*.
* [`ConfigurationConfigurator`](Diagnostish.Desktop/Composition/ConfigurationConfigurator.cs) — создаёт *`appsettings.json`* при первом запуске, читает конфигурацию, откатывается на значения по умолчанию при отсутствующем/повреждённом файле.
* [`Views/ConsoleViews`](Diagnostish.Desktop/Views/ConsoleViews/) — [`BaseConsolePrinter`](Diagnostish.Desktop/Views/ConsoleViews/Common/BaseConsolePrinter.cs) (*`Template Method`*) и конкретные [`HardwareConsolePrinter`, `OperatingSystemConsolePrinter`](Diagnostish.Desktop/Views/ConsoleViews/).
* [`FinalReportPrintDispatcher`](Diagnostish.Desktop/Views/FinalReportPrintDispatcher.cs) — рассылает готовый [`FinalReport`](Diagnostish.Domain/Models/Reports/FinalReport.cs) по всем зарегистрированным принтерам, позволяя добавлять новые способы вывода без изменения контроллера.
* [`DiagnosticController`](Diagnostish.Desktop/Controllers/DiagnosticController.cs) — тонкий асинхронный оркестратор: приветствие → сбор отчёта → вывод во все принтеры → ожидание выхода.
* [`Program.cs`](Diagnostish.Desktop/Program.cs) — точка сборки: логирование, конфигурация, DI-контейнер, обработка `Ctrl+C`, запуск контроллера.

### Diagnostish.Tests ###

**Юнит-тесты (*`xUnit + FluentAssertions + NSubstitute`*) для [`Infrastructure`](Diagnostish.Infrastructure/Diagnostish.Infrastructure.csproj):**

* [`ParserTests`](Diagnostish.Tests/Infrastructure/Shared/Common/Utils/ParserTests.cs) — безопасное приведение типов, включая некорректные CIM-даты.
* [`GpuFallBackProviderTests`](Diagnostish.Tests/Infrastructure/Providers/GpuFallBackProviderTests.cs) — полное покрытие композитной логики выбора источника видеопамяти: валидные/невалидные значения, отказ одного или обоих источников, отсутствие совпадения по имени адаптера, несколько видеокарт одновременно, проброс `CancellationToken`.

<div>
<br>
</div>

> [!NOTE]
> ## 🔭 Планы развития ##
> * Расширение композитных (fallback) провайдеров на другие компоненты.
> * Настраиваемое сочетание клавиш для остановки сканирования с возможностью изменения через *`appsettings.json`*.
> * Добавление новый диагностических данных (например, сетевые адаптеры, USB-устройства, службы Windows).
> * Принтеры для вывода отчёта в файл.