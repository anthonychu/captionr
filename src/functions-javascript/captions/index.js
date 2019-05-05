module.exports = async function (context, req) {
    const captions = req.body;

    if (process.env.AUTHORIZED_USER && process.env.AUTHORIZED_USER.toLowerCase() !== req.headers['x-ms-client-principal-name'].toLowerCase()) {
        context.res = {
            status: 403
        }
        return
    }

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