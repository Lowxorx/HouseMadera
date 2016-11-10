using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using HouseMadera.Modèles;
using Microsoft.Practices.ServiceLocation;

namespace HouseMadera.Vue___Modèle
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class VueModeleLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public VueModeleLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)

                SimpleIoc.Default.Register<ICommercialConnect, DesignCommercialConnect>();

            else

                SimpleIoc.Default.Register<ICommercialConnect, CommercialConnect>();


            SimpleIoc.Default.Register<VueModeleLogin>();

        }

        public VueModeleLogin VMLogin
        {
            get
            {
                return ServiceLocator.Current.GetInstance<VueModeleLogin>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}