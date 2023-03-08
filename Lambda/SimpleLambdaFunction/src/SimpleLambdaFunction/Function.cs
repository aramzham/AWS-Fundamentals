using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SimpleLambdaFunction;

public class Function
{
    public void FunctionHandler(Hello h, ILambdaContext context)
    {
        context.Logger.LogInformation($"Hello from {h.World}");
    }
}

public class Hello
{
    public string World { get; set; } = default!;
}