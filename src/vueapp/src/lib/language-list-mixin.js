import axios from 'axios'
import constants from './constants'

export default {
  data() {
    return {
      fromLanguages: [],
      toLanguages: []
    }
  },
  methods: {
    async getLanguages() {
      const languages = (await axios.get(`${constants.apiBaseUrl}/api/languages`)).data
      return {
        from: languages,
        to: languages
      }
    }
  },
  async mounted() {
    const languages = await this.getLanguages()
    this.fromLanguages = languages.from
    this.toLanguages = languages.to
  }
}