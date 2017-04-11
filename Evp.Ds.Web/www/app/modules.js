(function (angular) {
  'use strict';

  angular.module('evp.modules',
    [
      'ui.router',
      'evp.main'
    ]);

  angular.module('evp.main.controllers', []);
  angular.module('evp.main.services', []);
  angular.module('evp.main', [
    'evp.main.controllers',
    'evp.main.services'
  ]);

})(angular);