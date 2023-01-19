﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CONTROLDEINGRESOS.Models
{
    public class VisitanteFrecuente
    {
        public Int64 cedula { get; set; }
        public string nombre { get; set; }
        public string arl { get; set; }
        public string empleadoAutoriza { get; set; }
        public string motivoVisita { get; set; }
        public string empresa { get; set; }
        public bool Frecuente { get; set; }
        public string placa { get; set; }
        public string fechaIniFrecuente { get; set; }
        public string fechaFinFrecuente { get; set; }


        public VisitanteFrecuente()
        {
            cedula = 0;
            nombre = "";
            arl = "";
            empleadoAutoriza = "";
            motivoVisita = "";
            placa = "";
            empresa = "";
            Frecuente = false;
            fechaIniFrecuente = "";
            fechaFinFrecuente = "";
           

        }


    }
}