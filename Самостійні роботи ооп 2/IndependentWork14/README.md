# Самостійна робота №14: Кейси Polly/Retry — політики відмовостійкості

## Опис проєкту
Консольний застосунок на C# (.NET) із використанням бібліотеки **Polly v8** (`ResiliencePipelineBuilder`), що демонструє три практичні сценарії відмовостійкості:
1. **DatabaseRetryScenario** — повторні спроби з експоненційною затримкою та Jitter при збоях БД.
2. **PaymentCircuitBreakerScenario** — розмикач ланцюга (`Circuit Breaker`) та резервна відповідь (`Fallback`) при відмові платіжного шлюзу.
3. **ReportTimeoutScenario** — обмеження часу очікування (`Timeout`) у комбінації з `Retry` при зависанні генерації звіту.

Детальний аналіз сценаріїв наведено у файлі [REPORT.md](REPORT.md).

## Запуск
```bash
dotnet restore
dotnet run
```
