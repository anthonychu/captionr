# CaptionR

Serverless real-time captioning and translation using:

* Microsoft Azure Cognitive Services - [Speech-to-Text JavaScript SDK](https://www.npmjs.com/package/microsoft-cognitiveservices-speech-sdk)
* [Vue.js](https://vuejs.org/)
* [Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/?WT.mc_id=captionr-github-antchu)
* [Azure SignalR Service](https://docs.microsoft.com/en-us/azure/azure-signalr/?WT.mc_id=captionr-github-antchu)

## Project setup

The app has 2 main projects:
* Vue.js frontend in `src/vueapp`
* Azure Functions backend in `src/javascript-function`

Additionally, you'll need a Cognitive Services Speech-to-Text API key ([free trial here](https://azure.microsoft.com/en-us/try/cognitive-services/?WT.mc_id=captionr-github-antchu)).

And you'll need to create an instance of Azure SignalR Service (free tier available).

### Vue.js app

Install npm packages and start the Vue.js dev server:

```bash
npm install
npm run serve
```

### Azure Function app

Create a file named local.settings.json (copy from local.settings.sample.json). Fill in the SignalR Service connection string.

With the Azure Functions Core Tools installed, run the function app:

```bash
func extensions install
func start
```

