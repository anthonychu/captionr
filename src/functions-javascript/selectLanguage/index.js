const constants = require('../common/constants');

module.exports = async function (context, req) {
    const { languageCode, userId } = req.body;
    const signalRGroupActions =
        constants.languageCodes.map(lc => ({
            userId: userId,
            groupName: lc,
            action: (lc === languageCode) ? 'add' : 'remove'
        }));
    context.bindings.signalRGroupActions = signalRGroupActions;
};