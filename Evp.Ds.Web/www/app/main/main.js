(function (angular) {
  'use strict';

  StateConfiguration.$inject = ['$stateProvider'];
  // ReSharper disable once InconsistentNaming
  function StateConfiguration($stateProvider) {
    $stateProvider
      .state('main',
        {
          url: '/',
          templateUrl: 'www/app/main/views/main.html',
          controller: 'MainController',
          controllerAs: 'mainVm'
        });
  }

  angular.module('evp.main')
    .config(StateConfiguration);

})(angular);