var exec = require('cordova/exec');
var idle = function () {};

module.exports = {
    applyOrientationSettings: function (onSuccess, onError) {
        exec(onSuccess || idle, onError || idle, "OrientationSupport", "applyOrientationSettings", []);
    }
};