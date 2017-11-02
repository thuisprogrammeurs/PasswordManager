﻿using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using Zzz.Core.Contracts.ViewModels;
using Zzz.Core.Models;

namespace Zzz.Core.ViewModels
{
    public class AuthWizardViewModel : BaseViewModel, IAuthWizardViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private enum WizardSteps { Intro = 0, SelectAuthMethod, ClassicAuth, PictureAuth, FingerPrintAuth };
        private WizardSteps currentWizardStep;


        public AuthWizardViewModel(IMvxMessenger messenger, IMvxNavigationService navigation) : base(messenger)
        {
            _navigationService = navigation;

            // Init.
            currentWizardStep = WizardSteps.Intro;

            NextStepCommand = new MvxCommand(ShowWizard);
            PreviousStepCommand = new MvxCommand(ShowWizard);
        }

        public IMvxCommand NextStepCommand { get; private set; }

        public IMvxCommand PreviousStepCommand { get; private set; }

        public void StartAuthWizard()
        {
            ShowWizard();
        }

        private async void ShowWizard()
        {
            AuthSetting authSetting = new AuthSetting();
            bool isCompleted = false;

            while (isCompleted == false)
            {
                switch (currentWizardStep)
                {
                    case WizardSteps.Intro:
                        authSetting = await _navigationService.Navigate<AuthWizardWelcomeViewModel, AuthSetting, AuthSetting>(authSetting);
                        if (authSetting.IsOk == true)
                        {
                            currentWizardStep = WizardSteps.SelectAuthMethod;
                        }

                        break;

                    case WizardSteps.SelectAuthMethod:
                        authSetting = await _navigationService.Navigate<SelectAuthMethodViewModel, AuthSetting, AuthSetting>(authSetting);
                        if (authSetting.MainAuthentication == AuthOption.Classic)
                        {
                            currentWizardStep = WizardSteps.ClassicAuth;
                        }
                        else
                        {
                            currentWizardStep = WizardSteps.PictureAuth;
                        }

                        break;

                    case WizardSteps.ClassicAuth:
                        authSetting = await _navigationService.Navigate<ClassicAuthViewModel, AuthSetting, AuthSetting>(authSetting);
                        if (authSetting.IsOk)
                        {
                            //currentWizardStep = WizardSteps.FingerPrintAuth;
                            // Do something next.
                        }
                        else
                        {
                            currentWizardStep = WizardSteps.SelectAuthMethod;
                        }
                        break;

                    case WizardSteps.PictureAuth:
                        authSetting = await _navigationService.Navigate<PictureAuthViewModel, AuthSetting, AuthSetting>(authSetting);
                        if (authSetting.IsOk)
                        {
                            //currentWizardStep = WizardSteps.FingerPrintAuth;
                            // Do something next.
                        }
                        else
                        {
                            currentWizardStep = WizardSteps.SelectAuthMethod;
                        }
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
