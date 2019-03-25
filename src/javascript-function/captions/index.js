module.exports = async function (context, req) {
    const captions = req.body;

    const languageCaptions = [];
    for (const language in captions.languages) {
        languageCaptions.push({
            language,
            offset: captions.offset,
            text: captions.languages[language]
        });
    }

    return languageCaptions.map(lc => ({
        target: "newCaption",
        groupName: lc.language,
        arguments: [ lc ]
    }));
} 