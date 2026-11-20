using System;
using IndependentWork14;

Console.OutputEncoding = System.Text.Encoding.UTF8;

DatabaseRetryScenario.Run();
PaymentCircuitBreakerScenario.Run();
ReportTimeoutScenario.Run();
