# Diagnostish #

Портативная консольная утилита на `C#` для автоматизации первичной диагностики и сбора технических/программных характеристик компьютера. Разработана специально для запуска с сервисных флеш-накопителей.

> [!IMPORTANT]
> ## ⚖️ Лицензия ##
> Этот проект распространяется под лицензией *[`MIT`](LICENSE.txt)*. Вы можете свободно использовать, изменять и распространять код.

> [!WARNING]
> ## 📋 Требования ##
> * ***ОС:*** Windows 7 SP1 и выше.
> * ***Права:*** Администратор (требуется для доступа к WMI-сенсорам; манифест приложения запрашивает повышение автоматически).
> * ***.NET Runtime:*** Встроен (self-contained билд).

## 🚀 Основной функционал ##

* ***Сбор аппаратных данных.*** Низкоуровневый опрос компонентов (процессор, оперативная память, видеокарты, накопители, материнская плата, BIOS) и данных операционной системы через *`Windows Management Instrumentation (WMI).`*
* ***Многоуровневая отказоустойчивость.***  Безопасный парсинг данных ([`Parser`](Diagnostish.Infrastructure/Shared/Utils/Parser.cs)) и безопасное выполнение WMI-запросов ([`ExecutorWmi`](Diagnostish.Infrastructure/Shared/Wmi/Executor/ExecutorWmi.cs)) с раздельной обработкой ошибок доступа, таймаутов и повреждённых записей. Отсутствие или некорректность отдельных WMI-полей не прерывает сбор — утилита продолжает работу, фиксируя предупреждения (*`Warnings`*) и критические ошибки (*`CriticalErrors`*) в итоговом отчёте.
* ***Автоматизация прав доступа.*** Встроенный манифест приложения (*`UAC`*) запрашивает права администратора автоматически, что необходимо для корректного чтения системных WMI-датчиков.
* ***Автономность.*** Утилита собирается в один исполняемый файл со всеми зависимостями. Работает на любом ПК с *`Windows`* без предварительной установки *`.NET`*.
* ***Информативность.*** Результаты сканирования структурированы и выводятся в кастомном цветовом интерфейсе консоли.
* ***Логирование.*** Все события и ошибки записываются в директорию *`/logs`* в формате текстового журнала (*`Serilog`*). В случае сбоя лог-файл помогает быстро локализовать проблему.

## 🏗 Архитектура и технологии ##

* Проект спроектирован по принципам *`Clean Architecture (Domain → Application/Infrastructure → Desktop)`* с соблюдением *`SOLID`*, что обеспечивает независимость бизнес-логики от конкретных технологий сбора и вывода данных, а также лёгкость расширения и тестирования.
* ***Технологический стек:*** *`C# (.NET 10)`*, *`Serilog`*, *`System.Management (WMI)`*, *`Microsoft.Extensions.DependencyInjection`*, *`xUnit + FluentAssertions`*.

> [!TIP]
> ## 📦 Как использовать ##
> * Скачайте последний релиз утилиты во вкладке `Releases`.
> * Поместите скомпилированный `.exe` на сервисную флешку.
> * Запустите утилиту на целевом ПК — она автоматически запросит права администратора, соберёт диагностические данные и выведет их в консольном интерфейсе.

## ℹ️ Структура ##
###  Diagnostish.Domain ###

  **Ядро приложения — не зависит ни от одного другого проекта решения.**
  
  * [`Entities`](Diagnostish.Domain/Models/Entities/) — доменные сущности с провалидированными данными (*`CpuInfo`*, *`RamInfo`*, *`GpuInfo`*, *`StorageDriveInfo`*, *`BiosInfo`*, *`BaseBoardInfo`*, *`OperatingSystemInfo`*).
  * [`Reports`](Diagnostish.Domain/Models/Reports/) — плоские модели для представления результатов (*`HardwareReport`*, *`OperatingSystemReport`*, объединяющий их *`FinalReport`*), а также базовый *`IssuesReport`* со списками *`Warnings/CriticalErrors`*.
  * [`ProvideResult`](Diagnostish.Domain/Common/ProvideResult.cs) — единый «конверт» результата на каждом этапе конвейера: данные (или *`null`* при полном сбое) плюс списки предупреждений и критических ошибок.
  * [`Interfaces`](Diagnostish.Domain/Interfaces/) — контракты, не привязанные ни к *`WMI`*, ни к консоли:
    * [`IProvideDiagnosticInfo`](Diagnostish.Domain/Interfaces/IProvideDiagnosticInfo.cs) — сбор сырых данных;
    * [`IAnalyzeDiagnosticInfo`](Diagnostish.Domain/Interfaces/IAnalyzeDiagnosticInfo.cs) — анализ и валидация;
    * [`IReportMapper`](Diagnostish.Domain/Interfaces/IReportMapper.cs) — перенос провалидированных данных в отчёт;
    * [`IReportPrinter`](Diagnostish.Domain/Interfaces/IReportPrinter.cs) — вывод готового отчёта;
    * [`IUserInterface`](Diagnostish.Domain/Interfaces/IUserInterface.cs) — взаимодействие с пользователем (приветствие, ожидание выхода).

### Diagnostish.Infrastructure ###

**Реализация сбора данных через *`WMI`*. Зависит только от [`Domain`](Diagnostish.Domain/Diagnostish.Domain.csproj).**

* [`BaseWmiProvider`](Diagnostish.Infrastructure/Providers/Common/BaseWmiProvider.cs) — абстрактный базовый класс (*`Template Method`*), инкапсулирующий общую механику WMI-запроса; конкретный провайдер описывает только *`BuildQuery()`*, *`ContextName`* и *`Map(...)`*.
* [`HardwareInfoProviders`](Diagnostish.Infrastructure/Providers/HardwareInfoProviders/), [`OperatingSystemProviders`](Diagnostish.Infrastructure/Providers/OperatingSystemInfoProviders/) — конкретные WMI-провайдеры (*`CpuInfoWmiProvider`*, *`RamInfoWmiProvider`* и т.д.) и их «сырые» *`DTO`* (*`RawCpuInfo`*, *`RawRamInfo`* и т.д.) в подпапке [`RawHardwareInfo`](Diagnostish.Infrastructure/Providers/HardwareInfoProviders/RawHardwareInfo/).
* [`Analyzers`](Diagnostish.Infrastructure/Analyzers/) — бизнес-логика валидации: превращение сырых данных в доменные сущности с одновременной генерацией предупреждений. Общие проверки вынесены в [`AnalyzerValidationExtensions`](Diagnostish.Infrastructure/Analyzers/Common/AnalyzerValidationExtensions.cs) (*`GetValueOrWarning`* для строк, чисел через *`INumber<T>`* и дат). Тексты предупреждений сгруппированы по компоненту в [`Messages`](Diagnostish.Infrastructure/Analyzers/HardwareInfoAnalyzers/Messages/).
* [`Wmi`](Diagnostish.Infrastructure/Shared/Wmi/) — [`ExecutorWmi`](Diagnostish.Infrastructure/Shared/Wmi/Executor/ExecutorWmi.cs)/[`IExecutorWmi`](Diagnostish.Infrastructure/Shared/Wmi/Executor/IExecutorWmi.cs) (безопасное выполнение запросов с раздельной обработкой *`UnauthorizedAccessException`*, *`ManagementException`* по кодам *`AccessDenied/Timedout`*), [`ExecutorMessages`](Diagnostish.Infrastructure/Shared/Wmi/Executor/ExecutorMessages.cs), [`WmiSettings`](Diagnostish.Infrastructure/Shared/Wmi/WmiSettings.cs).
* [`Utils`](Diagnostish.Infrastructure/Shared/Utils/) — [`Parser`](Diagnostish.Infrastructure/Shared/Utils/Parser.cs) (безопасное приведение WMI-значений к nullable-типам **C#**) и [`ByteConverter`](Diagnostish.Infrastructure/Shared/Utils/ByteConverter.cs) (перевод байт в гигабайты).

### Diagnostish.Application ###

**Оркестрация бизнес-процесса. Зависит только от [`Domain`](Diagnostish.Domain/Diagnostish.Domain.csproj).**

Pipelines/ComponentPipeline<TReport> — типизированная обёртка над Action<TReport>, представляющая полный цикл «собрать → проанализировать → замапить» для одного компонента.
Mappers/ — реализации IReportMapper<TReport, TInfo> для каждого компонента; общая логика (перенос Warnings/CriticalErrors, безопасное извлечение данных) вынесена в Mappers/Common/MapperExtensions.TryExtractData.
Services/ServicesAggregator — собирает FinalReport, прогоняя все зарегистрированные ComponentPipeline для аппаратной части и части ОС.

### Diagnostish.Desktop ###

***`Composition Root`* и точка входа. Единственный проект, которому разрешено знать одновременно про [`Infrastructure`](Diagnostish.Infrastructure/Diagnostish.Infrastructure.csproj) и конкретные реализации представления.**

Composition/ServiceCollectionExtensions — универсальные generic-методы регистрации: AddComponent<TReport, TRaw, TInfo, TProvider, TAnalyzer, TMapper>() регистрирует провайдер, анализатор, маппер и собирает из них ComponentPipeline одной строкой; AddPrinter<TReport, TPrinter>() регистрирует принтер отчёта.
Composition/LoggerConfigurator — настройка Serilog (запись в /logs с ротацией по дням).
Views/Common/ReportPrinter<TReport> — базовый класс принтера (Template Method: PrintReport + общий вывод Warnings/CriticalErrors).
Views/HardwareInfoPrinters/, Views/OperatingSystemInfoPrinters/ — консольные принтеры отчётов.
Views/PrintersAggregator — рассылает готовый FinalReport по всем зарегистрированным принтерам (позволяет добавлять новые способы вывода — например, в файл — не меняя контроллер).
Controllers/DiagnosticController — тонкий оркестратор: приветствие → сбор отчёта → вывод во все принтеры → ожидание выхода.
Program.cs — точка сборки: настройка DI-контейнера, регистрация всех компонентов и принтеров, запуск контроллера.

### Diagnostish.Tests ###

**Юнит-тесты (*`xUnit + FluentAssertions1`*) для [`Infrastructure`](Diagnostish.Infrastructure/Diagnostish.Infrastructure.csproj) — на данный момент покрывают [`Parser`](Diagnostish.Infrastructure/Shared/Utils/Parser.cs) (безопасное приведение типов из *`WMI`*, включая некорректные CIM-даты).**

> [!NOTE]
> ## 🔭 Планы развития ##
> * Вынос настроек (таймаут WMI-запросов и т.п.) в конфигурацию приложения.
> * Резервный сбор данных о видеопамяти через реестр — для видеокарт, отдающих через *`WMI`* некорректный объём из-за ограничения *`uint32`*.
> * Переход на асинхронный сбор данных и параллельное выполнение независимых WMI-запросов.
> * Расширение набора провайдеров данных (реестр как альтернативный/резервный источник наравне с *`WMI`*) и принтеров (вывод в файл).
