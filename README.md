# Image auto translation for Sitefinity

When you upload image and publish it, it will automatically create another translations for this image

## Install

1.  Copy file /src/AutoImageTranslator.cs into your solution
2.  In your Global.asax.cs file add this code:

```
protected void Application_Start(object sender, EventArgs e)
{
    Telerik.Sitefinity.Abstractions.Bootstrapper.Initialized += Bootstrapper_Initialized;
}
protected void Bootstrapper_Initialized(object sender, Telerik.Sitefinity.Data.ExecutedEventArgs args)
{
    if (args.CommandName == "Bootstrapped")
    {
        EventHub.Subscribe<IDataEvent>(AutoImageTranslator.Action);
    }
}
```