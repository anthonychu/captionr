const constants = {
  languages: [
    'en-US',
    'fr-FR',
    'es-ES',
    'ko-KO',
    'ja-JP',
    'de-DE'
  ]
};

constants.languageCodes = constants.languages.map(l => l.substring(0, 2));

module.exports = constants;