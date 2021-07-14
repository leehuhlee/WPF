using Ninject;
using System;

namespace Dialog.Core
{
    public static class IoC
    {
        #region Public Properties
        public static IKernel Kernel { get; private set; } = new StandardKernel();

        public static IUIManager UI => IoC.Get<IUIManager>();
        #endregion

        #region Construction
        public static void Setup()
        {
            BindViewModels();
        }

        private static void BindViewModels()
        {
            Kernel.Bind<ApplicationViewModel>().ToConstant(new ApplicationViewModel());
        }
        #endregion

        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }
    }
}
