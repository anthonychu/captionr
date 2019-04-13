<template>
  <div class="caption-host">
    <div v-if="!started">
      <h1>Host a session</h1>
      <div><input type="password" v-model="key" placeholder="Cognitive Services Speech API Key" /></div>
      <div>
        <select v-model="fromLanguage">
          <option v-for="lang in fromLanguages" :value="lang" :key="lang">
            {{ lang }}
          </option>
        </select>
        <button @click="start">start</button>
      </div>
    </div>
    <div v-else>
      <button @click="stop">stop</button>
      <div id="currentSentence" class="caption">
        {{ currentSentence }}
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import constants from '../lib/constants'
import Translator from '../lib/translator'
import languageListMixin from '../lib/language-list-mixin'

const speechApiKeyLocalStorageKey = 'speechApiKey'

export default {
  mixins: [ languageListMixin ],
  data() {
    return {
      key: window.localStorage.getItem(speechApiKeyLocalStorageKey) || '',
      region: `${constants.region}`,
      currentSentence: '',
      started: false,
      fromLanguage: 'en-US'
    }
  },
  watch: {
    key(newKey) {
      window.localStorage.setItem(speechApiKeyLocalStorageKey, newKey)
    }
  },
  created() {
    this.translator = new Translator(function(captions) {
      this.currentSentence = captions.original
      axios.post(`${constants.apiBaseUrl}/api/captions`,
        captions.translations,
        { withCredentials: true })
    }.bind(this))
  },
  methods: {
    start() {
      this.translator.start({
        key: this.key,
        region: this.region,
        fromLanguage: this.fromLanguage,
        toLanguages: this.toLanguages
      })
      this.started = true
    },
    stop() {
      this.started = false
      this.currentSentence = ''
      this.translator.stop()
    }
  },
  beforeDestroy() {
    this.translator.stop()
  }
}
</script>

<style scoped>
input[type=password] {
  width: 600px;
}
</style>