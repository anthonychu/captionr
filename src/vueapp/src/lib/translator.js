import { 
  AudioConfig, SpeechTranslationConfig, TranslationRecognizer, ResultReason 
} from 'microsoft-cognitiveservices-speech-sdk'

class Translator {
  _recognizer
  _callback

  constructor(callback) {
    this._callback = callback
  }

  start(options) {
    const alreadyStarted = !!this._recognizer
    if (alreadyStarted) {
      return
    }

    const audioConfig = AudioConfig.fromDefaultMicrophoneInput()
    const speechConfig = SpeechTranslationConfig.fromSubscription(options.key, options.region)

    speechConfig.speechRecognitionLanguage = options.fromLanguage
    for (const lang of options.toLanguages) {
      speechConfig.addTargetLanguage(lang)
    }

    this._recognizer = new TranslationRecognizer(speechConfig, audioConfig)
    this._recognizer.recognizing = this._recognizer.recognized = recognizerCallback.bind(this)
    this._recognizer.startContinuousRecognitionAsync()

    function recognizerCallback(s, e) {
      const result = e.result
      const reason = ResultReason[result.reason]
      if (reason !== 'TranslatingSpeech' && reason !== 'TranslatedSpeech') {
        return
      }

      const captions = {
        offset: result.offset,
        languages: {}
      }
      captions.languages[getMainLanguage(options.fromLanguage)] = result.text

      //console.log(result.text)
      for (const lang of options.toLanguages) {
        const mainLang = getMainLanguage(lang)
        const caption = result.translations.get(mainLang)
        captions.languages[mainLang] = caption
      }

      this._callback({
        original: result.text,
        translations: captions
      })
      // axios.post(`${apiBaseUrl}/api/captions`, captions)
    }

    function getMainLanguage(lang) {
      return lang.substring(0, 2)
    }
  }
  
  stop() {
    this._recognizer.stopContinuousRecognitionAsync(
      stopRecognizer.bind(this),
      function (err) {
        stopRecognizer()
        console.error(err)
      }.bind(this)
    )

    function stopRecognizer() {
      this._recognizer.close()
      this._recognizer = undefined
      console.log('stopped')
    }
  }
}

export default Translator