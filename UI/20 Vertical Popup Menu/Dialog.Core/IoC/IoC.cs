using Ninject;
using System;

namespace Dialog.Core
{
    public static class IoC
    {
        #region Public Properties
        public static IKernel Kernel { get; private set; } = new StandardKernel();
        #endregion

        #region Construction
        public static void Setup()
        {
            // Bind all required view models
            BindViewModels();
        }

        private static void BindViewModels()
        {
            // Bind to a single instance of Application view model
            Kernel.Bind<ApplicationViewModel>().ToConstant(new ApplicationViewModel());
        }
        #endregion

        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }
    }
}
