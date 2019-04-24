﻿using SportLeagueRD.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SportLeagueRD.View{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class view_eventos : ContentPage{
        #region VARIABLES
        private viewmodel_eventos viewmodel = null;
        #endregion

        #region CONSTRUCTOR
        public view_eventos(){
            InitializeComponent();

            viewmodel = new viewmodel_eventos();
            BindingContext = viewmodel;
        }
        #endregion

        #region METODOS
        protected override void OnAppearing(){
            base.OnAppearing();
            //INICIA EL MESDAGINCENTER CUANDO LA VENTANA ESTA VISIBLE
            viewmodel.StarMessaginCenter();
            //LLENA LA TABLA CON LOS PRIMEROS REGISTROS LA PRIMERA VEZ QUE ESTA PAGINA APAREZCA
            viewmodel.LlenarTablaPrimeraVez();
        }

        protected override void OnDisappearing(){
            base.OnDisappearing();
            //DETIENE EL MESDAGINCENTER CUANDO LA VENTANA ESTA INVISIBLE PARA AHORRAR MEMORIA
            viewmodel.StopMessaginCenter();
        } 
        #endregion
	}
}