const supportedLanguages = [
  'en-US',
  'fr-FR',
  'es-ES',
  'ko-KO'
]

export default {
  data() {
    return {
      fromLanguages: [],
      toLanguages: []
    }
  },
  methods: {
    async getLanguages() {
      return {
        from: supportedLanguages,
        to: supportedLanguages
      }
    }
  },
  async mounted() {
    const languages = await this.getLanguages()
    this.fromLanguages = languages.from
    this.toLanguages = languages.to
  }
}