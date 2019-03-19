<template>
  <div class="caption-join">
    <div id="language-select">
      <select v-model="toLanguage">
        <option v-for="lang in toMainLanguages" :value="lang" :key="lang">
          {{ lang }}
        </option>
      </select>
    </div>
    <div id="captions-text" class="caption">
      <div v-for="caption in captions" :key="caption.offset">{{ caption.text }}</div>
    </div>
  </div>
</template>

<script>
import Vue from 'vue'
import constants from '../lib/constants'
import * as signalR from '@aspnet/signalr'
import languageListMixin from '../lib/language-list-mixin'

export default {
  mixins: [ languageListMixin ],
  data() {
    return {
      code: '',
      captions: [],
      toLanguage: 'en'
    }
  },
  computed: {
    toMainLanguages() {
      return this.toLanguages.map(l => l.substring(0, 2)).sort()
    }
  },
  async mounted() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(`${constants.apiBaseUrl}/api`)
      .build()

    this.connection.on('newCaption', onNewCaption.bind(this))

    await this.connection.start()
    console.log('connection started')

    const captionsMap = {}
    function onNewCaption(caption) {
      let localCaption = captionsMap[caption.offset]
      if (!localCaption) {
        localCaption = captionsMap[caption.offset] = {
          offset: caption.offset,
          text: ''
        }
        this.captions.push(localCaption)
      }
      localCaption.text = caption.languages[this.toLanguage]

      Vue.nextTick(function() {
        window.scrollTo(0, document.body.scrollHeight || document.documentElement.scrollHeight)
      })
    }
  },
  async beforeDestroy() {
    if (this.connection) {
      await this.connection.stop()
      console.log('connection stopped')
    }
  }
}
</script>

<style scoped>
#language-select {
  position: fixed;
  top: 0;
  width: 100%;
  padding-top: 120px;
  padding-bottom: 6px;
  background-color: white;
}

#captions-text {
  padding-top: 140px;
  padding-bottom: 60px
}
</style>
