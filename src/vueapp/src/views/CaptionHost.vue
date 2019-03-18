<template>
  <div class="caption-host">
    <div v-if="!started">
      <h1>Host a session</h1>
      <div><input type="password" v-model="key" /></div>
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

export default {
  mixins: [ languageListMixin ],
  data() {
    const translator = new Translator(function(captions) {
      this.currentSentence = captions.original
      axios.post(`${constants.apiBaseUrl}/api/captions`, captions.translations)
    }.bind(this))

    return {
      translator,
      key: '',
      region: 'westus',
      currentSentence: '',
      started: false,
      fromLanguage: 'en-US'
    }
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