using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using FunctionAppContagemOutput.Contagem;

namespace FunctionAppContagemOutput;

public static class GeracaoEventos
{
    private const string EVENTHUB_NAME = "hub-contagem";
    private static readonly Contador CONTADOR = new();

    [Function(nameof(GeracaoEventos))]
    [EventHubOutput(EVENTHUB_NAME, Connection = "AzureEventHubsConnection")]
    public static ResultadoContador Run([TimerTrigger("*/5 * * * * *")] FunctionContext context)
    {
        var logger = context.GetLogger(nameof(GeracaoEventos));

        CONTADOR.Incrementar();
        string momento = $"Evento gerado em {DateTime.Now:HH:mm:ss}";

        logger.LogInformation(momento);
        logger.LogInformation($"Valor do contador = {CONTADOR.ValorAtual}");

        return new()
        {
            ValorAtual = CONTADOR.ValorAtual,
            Local = CONTADOR.Local,
            Kernel = CONTADOR.Kernel,
            Framework = CONTADOR.Framework,
            Mensagem = $"Origem: Function {nameof(GeracaoEventos)} | {momento}"
        };
    }
}