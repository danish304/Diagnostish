# Diagnostish #

Портативная консольная утилита на `C#` для автоматизации первичной диагностики и сбора технических/программных характеристик компьютера. Разработана специально для запуска с сервисных флеш-накопителей.

### ⚖️ Лицензия ###

Этот проект распространяется под лицензией *[`MIT`](LICENSE.txt)*. Вы можете свободно использовать, изменять и распространять код.

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


# ℹ️ Структура #
##  Diagnostish.Domain ##

Ядро приложения — не зависит ни от одного другого проекта решения.

Models/Entities/ — доменные сущности с провалидированными данными (CpuInfo, RamInfo, GpuInfo, StorageDriveInfo, BiosInfo, BaseBoardInfo, OperatingSystemInfo).
Models/Reports/ — плоские модели для представления результатов (HardwareReport, OperatingSystemReport, объединяющий их FinalReport), а также базовый IssuesReport со списками Warnings/CriticalErrors.
Common/ProvideResult<TInfo> — единый «конверт» результата на каждом этапе конвейера: данные (или null при полном сбое) плюс списки предупреждений и критических ошибок.
Interfaces/ — контракты, не привязанные ни к WMI, ни к консоли:
IProvideDiagnosticInfo<TRaw> — сбор сырых данных;
IAnalyzeDiagnosticInfo<TRaw, TInfo> — анализ и валидация;
IReportMapper<TReport, TInfo> — перенос провалидированных данных в отчёт;
IReportPrinter<TReport> — вывод готового отчёта;
IUserInterface — взаимодействие с пользователем (приветствие, ожидание выхода).

* ## Diagnostish.Infrastructure ##

Реализация сбора данных через WMI. Зависит только от Domain.

Providers/Common/BaseWmiProvider<TRawInfo> — абстрактный базовый класс (Template Method), инкапсулирующий общую механику WMI-запроса; конкретный провайдер описывает только BuildQuery(), ContextName и Map(...).
Providers/HardwareInfoProviders/, Providers/OperatingSystemInfoProviders/ — конкретные WMI-провайдеры (CpuInfoWmiProvider, RamInfoWmiProvider и т.д.) и их «сырые» DTO (RawCpuInfo, RawRamInfo и т.д.) в подпапке RawHardwareInfo/.
Analyzers/ — бизнес-логика валидации: превращение сырых данных в доменные сущности с одновременной генерацией предупреждений. Общие проверки вынесены в Analyzers/Common/AnalyzerValidationExtensions (GetValueOrWarning для строк, чисел через INumber<T> и дат). Тексты предупреждений сгруппированы по компоненту в Messages/.
Shared/Wmi/ — ExecutorWmi/IExecutorWmi (безопасное выполнение запросов с раздельной обработкой UnauthorizedAccessException, ManagementException по кодам AccessDenied/Timedout), ExecutorMessages, WmiSettings.
Shared/Utils/ — Parser (безопасное приведение WMI-значений к nullable-типам C#) и ByteConverter (перевод байт в гигабайты).

* ## Diagnostish.Application ##

Оркестрация бизнес-процесса. Зависит только от Domain.

Pipelines/ComponentPipeline<TReport> — типизированная обёртка над Action<TReport>, представляющая полный цикл «собрать → проанализировать → замапить» для одного компонента.
Mappers/ — реализации IReportMapper<TReport, TInfo> для каждого компонента; общая логика (перенос Warnings/CriticalErrors, безопасное извлечение данных) вынесена в Mappers/Common/MapperExtensions.TryExtractData.
Services/ServicesAggregator — собирает FinalReport, прогоняя все зарегистрированные ComponentPipeline для аппаратной части и части ОС.

* ## Diagnostish.Desktop ##

Composition Root и точка входа. Единственный проект, которому разрешено знать одновременно про Infrastructure и конкретные реализации представления.

Composition/ServiceCollectionExtensions — универсальные generic-методы регистрации: AddComponent<TReport, TRaw, TInfo, TProvider, TAnalyzer, TMapper>() регистрирует провайдер, анализатор, маппер и собирает из них ComponentPipeline одной строкой; AddPrinter<TReport, TPrinter>() регистрирует принтер отчёта.
Composition/LoggerConfigurator — настройка Serilog (запись в /logs с ротацией по дням).
Views/Common/ReportPrinter<TReport> — базовый класс принтера (Template Method: PrintReport + общий вывод Warnings/CriticalErrors).
Views/HardwareInfoPrinters/, Views/OperatingSystemInfoPrinters/ — консольные принтеры отчётов.
Views/PrintersAggregator — рассылает готовый FinalReport по всем зарегистрированным принтерам (позволяет добавлять новые способы вывода — например, в файл — не меняя контроллер).
Controllers/DiagnosticController — тонкий оркестратор: приветствие → сбор отчёта → вывод во все принтеры → ожидание выхода.
Program.cs — точка сборки: настройка DI-контейнера, регистрация всех компонентов и принтеров, запуск контроллера.

* ## Diagnostish.Tests ##

Юнит-тесты (xUnit + FluentAssertions) для Infrastructure — на данный момент покрывают Parser (безопасное приведение типов из WMI, включая некорректные CIM-даты).

## 📋 Требования ##

* ***ОС:*** Windows 7 SP1 и выше.
* ***Права:*** Администратор (требуется для доступа к WMI-сенсорам; манифест приложения запрашивает повышение автоматически).
* ***.NET Runtime:*** Встроен (self-contained билд).

## 📦 Как использовать ##

* Скачайте последний релиз утилиты во вкладке `Releases`.
* Поместите скомпилированный `.exe` на сервисную флешку.
* Запустите утилиту на целевом ПК — она автоматически запросит права администратора, соберёт диагностические данные и выведет их в консольном интерфейсе.

## 🔭 Планы развития ##

* Вынос настроек (таймаут WMI-запросов и т.п.) в конфигурацию приложения.
* Резервный сбор данных о видеопамяти через реестр — для видеокарт, отдающих через *`WMI`* некорректный объём из-за ограничения *`uint32`*.
* Переход на асинхронный сбор данных и параллельное выполнение независимых WMI-запросов.
* Расширение набора провайдеров данных (реестр как альтернативный/резервный источник наравне с *`WMI`*) и принтеров (вывод в файл).
