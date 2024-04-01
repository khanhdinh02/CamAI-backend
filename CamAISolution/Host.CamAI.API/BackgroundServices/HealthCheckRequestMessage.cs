using Infrastructure.MessageQueue;
using MassTransit;

namespace Host.CamAI.API.BackgroundServices;

[Publisher(PublisherConstant.HealthCheck)]
[MessageUrn("HealthCheckRequestMessage")]
public class HealthCheckRequestMessage;
