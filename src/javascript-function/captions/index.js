module.exports = async function (context, req) {
    const captions = req.body;

    const languageCaptions = Object.keys(captions.languages).map(captionLanguage => ({
        language: captionLanguage,
        offset: captions.offset,
        text: captions.languages[captionLanguage]
    }));

    const signalRMessages = languageCaptions.map(lc => ({
        target: 'newCaption',
        groupName: lc.language,
        arguments: [ lc ]
    }));

    return signalRMessages;
};