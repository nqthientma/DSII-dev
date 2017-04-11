(function (angular) {
  'use strict';

  angular.module('evp',
      [
        'evp.modules'
      ])
    .config([
      '$compileProvider', '$logProvider', function ($compileProvider, $logProvider) {
        $compileProvider.debugInfoEnabled(false);
        $logProvider.debugEnabled(false);
      }
    ])
    .config([
      '$urlRouterProvider', '$locationProvider', function ($urlRouterProvider, $locationProvider) {
        $urlRouterProvider.otherwise('/');
        $locationProvider.html5Mode(false);
      }
    ])
    .run([
      '$rootScope', function ($rootScope) {
        $rootScope.$on('$stateChangeStart',
          function (event, toState, toStateParams, fromState, fromStateParams) {
            $rootScope.toState = toState;
            $rootScope.toStateParams = toStateParams;
            $rootScope.fromState = fromState;
            $rootScope.fromStateParams = fromStateParams;
          });
      }
    ]);

})(angular);