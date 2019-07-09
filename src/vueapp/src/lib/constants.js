export default {
  apiBaseUrl: process.env.VUE_APP_API_BASE_URL,
  region: process.env.VUE_APP_COGNITIVE_API_REGION,
  authProvider: process.env.VUE_APP_AUTH_PROVIDER,
  title: process.env.VUE_APP_TITLE || 'CaptionR'
}