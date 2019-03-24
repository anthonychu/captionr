const constants = require('../common/constants');

module.exports = async function (context, req) {
    const { languageCode, userId } = req.body;
    const signalRGroupActions =
        constants.languageCodes.map(lc => ({
            userId: userId,
            groupName: lc,
            action: (lc === languageCode) ? 'add' : 'remove'
        }));
        console.log(JSON.stringify(signalRGroupActions, null, 2));
    context.bindings.signalRGroupActions = signalRGroupActions;
};