using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using CapaDatos;
using Microsoft.AspNetCore.Mvc;
using CONTROLDEINGRESOS.Models;
using PRYHORASEXTRASV2.Models;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web.Mvc;
using ActionResult = System.Web.Mvc.ActionResult;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;
using NonActionAttribute = System.Web.Mvc.NonActionAttribute;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using CONTROLDEINGRESOS.Clases;

namespace CONTROLDEINGRESOS.Models
{
    public class Utility
    {
        /**
         Clase que se encargará de la lectura de cada uno de los 
         campos y filas del archivo excel. Simplificando 
         cada dato en su respectiva propiedad asociada
         para su mejor manipulación y lectura.
        **/

   
        private decimal cedula { get; set; }
       
        private string nombre { get; set; }
        private string arl { get; set; }
        private string empleadoAutoriza { get; set; }
        private string motivoVisita { get; set; }
        private string empresa { get; set; }
        private bool Frecuente { get; set; }
        private string placa { get; set; }
        private string fechaIniFrecuente { get; set; }
        private string fechaFinFrecuente { get; set; }



        //método que lee los archivos csv y pasa el contenido a un dataset.
        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }

                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    if (rows.Length > 1)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i].Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                }

            }


            return dt;
        }



        //método que leer las hojas del archivo excel, saca los campos, las filas y las incluye en un dataset.
        public static DataTable ConvertXSLXtoDataTable(string strFilePath, string connString, string usuario)
        {
            OleDbConnection oledbConn = new OleDbConnection(connString);
            Utility utilityProp = new Utility();
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
              



                oledbConn.Open();
                using (DataTable Sheets = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null))
                {

                    for (int i = 0; i < Sheets.Rows.Count; i++)
                    {
                        string worksheets = Sheets.Rows[i]["TABLE_NAME"].ToString();
                        OleDbCommand cmd = new OleDbCommand(String.Format("SELECT * FROM [{0}]", worksheets), oledbConn);
                        OleDbDataAdapter oleda = new OleDbDataAdapter();
                        oleda.SelectCommand = cmd;

                        oleda.Fill(ds);
                  
                    }

                    dt = ds.Tables[0];

                    //eliminar filas vacías dentro del datatable respuesta.
                    var dtResultado = dt.Rows.Cast<DataRow>()
                                      .Where(row => !Array.TrueForAll(row.ItemArray, value => { return value.ToString().Length == 0; }));

                    //regresar el resultado limpio al dt.
                    dt = dtResultado.CopyToDataTable();



                    //List<VisitanteFrecuente> listVisitanteFrecuente = new List<VisitanteFrecuente>();

                    foreach (DataRow row in dt.Rows)
                    {
                        // VisitanteFrecuente visitanteFre = new VisitanteFrecuente();

                        //Cedula
                        if (string.IsNullOrEmpty(row[0].ToString()))
                        {
                            utilityProp.cedula = 0;
                            //throw new Exception("El campo cedula no puede estar vacío");
                        }
                        else
                        {
                            string idFilter = row[0].ToString();
                            idFilter = idFilter.CleanId();

                            utilityProp.cedula = Int64.Parse(idFilter);                        
                        }

                        //cedula
                        //int valor;
                        //bool result;
                        //result = int.TryParse(row[0].ToString(), out valor);
                        //if (result == false)
                        //{
                        //    throw new Exception("El campo cedula solo recibe números");
                        //}
                        
                        //if (int.Parse(row[0].ToString()) > 0)
                        //{
                        //    utilityProp.cedula = int.Parse(row[0].ToString());
                        //}
                        //else { 
                        //throw new Exception("El campo cedula no puede estar vacío") ;  
                        //}
                                                                    


                        //nombre
                        if (string.IsNullOrEmpty(row[1].ToString()))
                        {
                            utilityProp.nombre = "VACÍO";
                            //throw new Exception("El campo nombre no puede estar vacío");
                        }
                        else
                        {
                            string nameFilter = row[1].ToString();
                            nameFilter =  nameFilter.CleanName();

                            utilityProp.nombre = nameFilter.ToUpper();
                                                   
                        }

                        //arl
                        utilityProp.arl = row[2].ToString().ToUpper();


                        //empleadoAutoriza
                        if (string.IsNullOrEmpty(row[3].ToString()))
                        {
                            utilityProp.empleadoAutoriza = "VACÍO";
                            //throw new Exception("El campo empleado autoriza no puede estar vacío");
                        }
                        else
                        {
                            utilityProp.empleadoAutoriza = row[3].ToString().ToUpper();
                        }


                        //motivoVisita
                        if (string.IsNullOrEmpty(row[4].ToString()))
                        {
                            utilityProp.motivoVisita = "VACÍO";
                            //throw new Exception("El campo motivo visita no puede estar vacío");
                        }
                        else
                        {
                            utilityProp.motivoVisita = row[4].ToString().ToUpper();
                        }

                        //empresa
                        if (string.IsNullOrEmpty(row[5].ToString()))
                        {
                            utilityProp.empresa = "VACÍO";
                            //throw new Exception("El campo empresa visita no puede estar vacío");
                        }
                        else
                        {
                            utilityProp.empresa = row[5].ToString().ToUpper();
                        }


                        //placa (no es campo obligatorio)
                         utilityProp.placa = row[6].ToString().ToUpper();
                       
                       

                        //fecha inicia
                        if (string.IsNullOrEmpty(row[7].ToString()))
                        {
                            utilityProp.fechaFinFrecuente = "SIN FECHA";
                            //throw new Exception("El campo fecha inicio no puede estar vacío");
                        }
                        else
                        {
                            utilityProp.fechaIniFrecuente = DateTime.Parse(row[7].ToString()).ToString("dd/MM/yyyy");
                        }

                        //fecha final
                        if (string.IsNullOrEmpty(row[8].ToString()))
                        {
                            utilityProp.fechaIniFrecuente = "SIN FECHA";
                            //throw new Exception("El campo fecha fin no puede estar vacío");
                        }
                        else
                        {
                            utilityProp.fechaFinFrecuente = DateTime.Parse(row[8].ToString()).ToString("dd/MM/yyyy");
                        }
                       
                       
                        List<Parametros> LstParametros = new List<Parametros>();
                        LstParametros.Add(new Parametros("@cedula", utilityProp.cedula, SqlDbType.VarChar));
                        LstParametros.Add(new Parametros("@nombre", utilityProp.nombre, SqlDbType.VarChar));
                        LstParametros.Add(new Parametros("@arl", utilityProp.arl, SqlDbType.VarChar));
                        LstParametros.Add(new Parametros("@Usuario", usuario, SqlDbType.VarChar));
                        LstParametros.Add(new Parametros("@empleadoAutoriza", utilityProp.empleadoAutoriza, SqlDbType.VarChar));
                        LstParametros.Add(new Parametros("@motivo", utilityProp.motivoVisita, SqlDbType.VarChar));
                        LstParametros.Add(new Parametros("@empresa", utilityProp.empresa, SqlDbType.VarChar));
                        LstParametros.Add(new Parametros("@placa", utilityProp.placa, SqlDbType.VarChar));
                        LstParametros.Add(new Parametros("@fechaIni", utilityProp.fechaIniFrecuente, SqlDbType.Date));
                        LstParametros.Add(new Parametros("@fechaFin", utilityProp.fechaFinFrecuente, SqlDbType.Date));
                        
                        string respuesta = Datos.SPGetEscalar("SP_GuardarExcelVisitanteFrecuente", LstParametros).ToString();




                        //decimal cedula = int.Parse(row[0].ToString());
                        //string nombre = row[1].ToString().ToUpper().ToUpper();
                        //string arl = row[2].ToString().ToUpper().ToUpper();
                        //string empleadoAutoriza = row[3].ToString().ToUpper();
                        //string motivo = row[4].ToString().ToUpper();
                        //string empresa = row[5].ToString().ToUpper();
                        //bool frecuente = bool.Parse(row[6].ToString());
                        //string placa = row[6].ToString().ToUpper();
                        //string fechaIni = DateTime.Parse(row[7].ToString().ToUpper()).ToString("dd/MM/yyyy");
                        //string fechafin = DateTime.Parse(row[8].ToString().ToUpper()).ToString("dd/MM/yyyy");


                        //listVisitanteFrecuente.Add(new VisitanteFrecuente()
                        //{
                        //    cedula = int.Parse(row[0].ToString().ToUpper()),
                        //    nombre = row[1].ToString().ToUpper(),
                        //    arl = row[2].ToString().ToUpper(),
                        //    empleadoAutoriza = row[3].ToString().ToUpper(),
                        //    motivoVisita = row[4].ToString().ToUpper(),
                        //    empresa = row[5].ToString().ToUpper(),
                        //    //Frecuente = bool.Parse(row[6].ToString()),
                        //    placa = row[6].ToString().ToUpper(),
                        //    fechaIniFrecuente = row[7].ToString().ToUpper(),
                        //    fechaFinFrecuente = row[8].ToString().ToUpper()
                        //});

                    }

                }


            }
            catch(Exception ex)
            {
                // throw new Exception(ex.Message);

                throw new ArgumentException(ex.Message);
            }
            finally
            {

                oledbConn.Close();
                oledbConn.Dispose();
            } 

            return dt;

        }




        //public static List<VisitanteFrecuente> lstVisitanteFrecuente(DataTable dt)

        //{
        //    var listExcel = (from row in dt.AsEnumerable()
        //                     select new VisitanteFrecuente()
        //                     {
        //                       cedula = int.Parse(row["cedula"].ToString()),
        //                       nombre = row["nombre"].ToString(),
        //                       arl = row["arl"].ToString(),
        //                       empleadoAutoriza = row["empleadoAutoriza"].ToString(),
        //                       motivoVisita = row["motivo"].ToString(),
        //                       empresa = row["empresa"].ToString(),
        //                       placa = row["placa"].ToString(),
        //                       fechaIniFrecuente = row["fechaIni"].ToString(),
        //                       fechaFinFrecuente = row["fechaFin"].ToString() DateTime.Parse(dr["fechaFinFrecuente"].ToString()).ToString("dd/MM/yyyy");
        //                     }
        //                     ).ToList();


        //    return listExcel;
        //}




    }
}