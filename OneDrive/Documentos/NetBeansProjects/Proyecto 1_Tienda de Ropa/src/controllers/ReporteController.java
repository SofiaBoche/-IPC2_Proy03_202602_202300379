/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package controllers;

/**
 *
 * @author tofab
 */
import models.Producto;
import models.Venta;
import java.io.File;
import java.io.PrintWriter;
import java.time.LocalDate;
public class ReporteController {

   public static void generarReporteStock(Producto[] productos, int contador) {

        try {

            String fecha = java.time.LocalDate.now().toString().replace("-", "_");
               File archivo = new File("reporte_stock_" + fecha + ".html");
            PrintWriter writer = new PrintWriter(archivo);

            writer.println("<html>");
            writer.println("<head>");
            writer.println("<title>Reporte de Inventario</title>");

            writer.println("<style>");

            writer.println("body{");
            writer.println("font-family: 'Segoe UI', Arial;");
            writer.println("background: linear-gradient(to right,#ece9e6,#ffffff);");
            writer.println("margin:0;");
            writer.println("padding:20px;");
            writer.println("}");

            writer.println("h1{");
            writer.println("text-align:center;");
            writer.println("color:#2c3e50;");
            writer.println("margin-bottom:10px;");
            writer.println("}");

            writer.println("p{");
            writer.println("text-align:center;");
            writer.println("color:gray;");
            writer.println("}");

            writer.println("table{");
            writer.println("border-collapse:collapse;");
            writer.println("margin:auto;");
            writer.println("width:85%;");
            writer.println("background:white;");
            writer.println("box-shadow:0px 4px 10px rgba(0,0,0,0.2);");
            writer.println("border-radius:8px;");
            writer.println("overflow:hidden;");
            writer.println("}");

            writer.println("th{");
            writer.println("background:#2c3e50;");
            writer.println("color:white;");
            writer.println("padding:14px;");
            writer.println("font-size:16px;");
            writer.println("}");

            writer.println("td{");
            writer.println("padding:12px;");
            writer.println("text-align:center;");
            writer.println("border-bottom:1px solid #ddd;");
            writer.println("}");

            writer.println("tr:hover{");
            writer.println("background:#f5f5f5;");
            writer.println("}");

            writer.println("</style>");

            writer.println("</head>");
            writer.println("<body>");

            writer.println("<h1> Inventario - Tienda de Ropa</h1>");
            writer.println("<p>Reporte de productos disponibles</p>");
            writer.println("<p>Fecha del reporte: " + LocalDate.now() + "</p>");

            writer.println("<table border='1'>");

            writer.println("<tr>");
            writer.println("<th>Codigo</th>");
            writer.println("<th>Nombre</th>");
            writer.println("<th>Categoria</th>");
            writer.println("<th>Precio</th>");
            writer.println("<th>Stock</th>");
            writer.println("</tr>");

            for (int i = 0; i < contador; i++) {

                writer.println("<tr>");
                writer.println("<td>" + productos[i].getCodigo() + "</td>");
                writer.println("<td>" + productos[i].getNombre() + "</td>");
                writer.println("<td>" + productos[i].getCategoria() + "</td>");
                writer.println("<td>" + productos[i].getPrecio() + "</td>");
                writer.println("<td>" + productos[i].getStock() + "</td>");
                writer.println("</tr>");

            }

            writer.println("</table>");

            writer.println("</body>");
            writer.println("</html>");

            writer.close();

            System.out.println("Reporte generado correctamente");

        } catch (Exception e) {

            System.out.println("Error al generar reporte");

        }

    }

               public static void generarReporteVentas(Venta[] ventas, int contadorVentas) {

    try {

        String fecha = java.time.LocalDate.now().toString().replace("-", "_");

        File archivo = new File("reporte_ventas_" + fecha + ".html");

        PrintWriter writer = new PrintWriter(archivo);

        writer.println("<html>");
        writer.println("<head>");
        writer.println("<title>Reporte de Ventas</title>");

        writer.println("<style>");
        writer.println("body{font-family:Arial;background:#f5f5f5;padding:20px;}");
        writer.println("h1{text-align:center;color:#333;}");
        writer.println("table{border-collapse:collapse;margin:auto;width:80%;background:white;}");
        writer.println("th{background:#2c3e50;color:white;padding:10px;}");
        writer.println("td{padding:10px;text-align:center;border-bottom:1px solid #ddd;}");
        writer.println("</style>");

        writer.println("</head>");
        writer.println("<body>");

        writer.println("<h1>Reporte de Ventas</h1>");

        writer.println("<table border='1'>");

        writer.println("<tr>");
        writer.println("<th>Codigo Producto</th>");
        writer.println("<th>Cantidad</th>");
        writer.println("<th>Total</th>");
        writer.println("<th>Fecha y Hora</th>");
        writer.println("</tr>");

        for (int i = 0; i < contadorVentas; i++) {

            writer.println("<tr>");

            writer.println("<td>" + ventas[i].getCodigoProducto() + "</td>");
            writer.println("<td>" + ventas[i].getCantidad() + "</td>");
            writer.println("<td>" + ventas[i].getTotal() + "</td>");
            writer.println("<td>" + ventas[i].getFechaHora() + "</td>");

            writer.println("</tr>");

        }

        writer.println("</table>");

        writer.println("</body>");
        writer.println("</html>");

        writer.close();

        System.out.println("Reporte de ventas generado");

    } catch (Exception e) {

        System.out.println("Error al generar reporte de ventas");

    }
               }
}
