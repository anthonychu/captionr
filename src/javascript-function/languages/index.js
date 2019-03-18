const constants = require('../common/constants');

module.exports = async function (context, req) {
    context.res.body = constants.languages;
};