/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "/";
/******/
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = "./src/ts/Index.ts");
/******/ })
/************************************************************************/
/******/ ({

/***/ "./src/ts/Classes/AlertInfo.ts":
/*!*************************************!*\
  !*** ./src/ts/Classes/AlertInfo.ts ***!
  \*************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
eval("\r\nObject.defineProperty(exports, \"__esModule\", { value: true });\r\nvar AlertInfo = /** @class */ (function () {\r\n    function AlertInfo(BootstrapType, Name) {\r\n        this.BootstrapType = BootstrapType;\r\n        this.Name = Name;\r\n    }\r\n    AlertInfo.GetRandomAlertInfo = function () {\r\n        var value = Math.random() * 100;\r\n        if (value < 25) {\r\n            return new AlertInfo(\"alert-success\", \"Success\");\r\n        }\r\n        if (value < 50) {\r\n            return new AlertInfo(\"alert-info\", \"Info\");\r\n        }\r\n        if (value < 75) {\r\n            return new AlertInfo(\"alert-warning\", \"Warning\");\r\n        }\r\n        return new AlertInfo(\"alert-danger\", \"Danger!\");\r\n    };\r\n    return AlertInfo;\r\n}());\r\nexports.AlertInfo = AlertInfo;\r\n\n\n//# sourceURL=webpack:///./src/ts/Classes/AlertInfo.ts?");

/***/ }),

/***/ "./src/ts/Index.ts":
/*!*************************!*\
  !*** ./src/ts/Index.ts ***!
  \*************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
eval("\r\nObject.defineProperty(exports, \"__esModule\", { value: true });\r\nvar AlertInfo_1 = __webpack_require__(/*! ./Classes/AlertInfo */ \"./src/ts/Classes/AlertInfo.ts\");\r\nvar alertList = document.querySelector(\"#AlertList\");\r\nvar addAlertButton = document.querySelector(\"#AddAlertButton\");\r\naddAlertButton.onclick = function () {\r\n    var newAlert = CreateAlert();\r\n    alertList.appendChild(newAlert);\r\n};\r\nvar deleteAlertsButton = document.querySelector(\"#DeleteAlertsButton\");\r\ndeleteAlertsButton.onclick = function () {\r\n    var numberOfAlerts = alertList.childNodes.length;\r\n    for (var index = 0; index < numberOfAlerts; index++) {\r\n        alertList.removeChild(alertList.children[0]);\r\n    }\r\n};\r\nfunction CreateAlert() {\r\n    var alertInfo = AlertInfo_1.AlertInfo.GetRandomAlertInfo();\r\n    var newAlert = document.createElement(\"div\");\r\n    newAlert.classList.add(\"alert\");\r\n    newAlert.classList.add(\"alert-dismissable\");\r\n    newAlert.classList.add(alertInfo.BootstrapType);\r\n    var closeLink = document.createElement(\"a\");\r\n    closeLink.href = \"#\";\r\n    closeLink.classList.add(\"close\");\r\n    closeLink.setAttribute(\"data-dismiss\", \"alert\");\r\n    // let closeLinkDataDismissAttr: Attr = document.createAttribute(\"data-dismiss\");\r\n    // closeLinkDataDismissAttr.value = \"alert\";\r\n    // closeLink.attributes.setNamedItem(closeLinkDataDismissAttr);\r\n    closeLink.innerHTML = \"&times;\";\r\n    newAlert.appendChild(closeLink);\r\n    var paragraph = document.createElement(\"p\");\r\n    paragraph.innerHTML = \"<strong>\" + alertInfo.Name + \" </strong> An alert was successfully added.\";\r\n    newAlert.appendChild(paragraph);\r\n    return newAlert;\r\n}\r\nfunction CreateAlertUsingTimer() {\r\n    setTimeout(function () {\r\n        CreateAlertUsingTimer();\r\n    }, 2000);\r\n    var newAlert = CreateAlert();\r\n    alertList.appendChild(newAlert);\r\n}\r\nCreateAlertUsingTimer();\r\n\n\n//# sourceURL=webpack:///./src/ts/Index.ts?");

/***/ })

/******/ });