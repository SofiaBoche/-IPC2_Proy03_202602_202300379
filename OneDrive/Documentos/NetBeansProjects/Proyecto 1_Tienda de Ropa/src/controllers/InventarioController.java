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
import controllers.ReporteController;
public class InventarioController {
public static Producto[] productos = new Producto[100];
    public static int contador = 0;
    public static Venta[] ventas = new Venta[100];
    public static int contadorVentas = 0;
   
    public static void agregarProducto(Producto p) {

        if (contador < productos.length) {

            productos[contador] = p;
            contador++;

        }

    }

    public static Producto buscarProducto(String codigo) {

        for (int i = 0; i < contador; i++) {

            if (productos[i].getCodigo().equals(codigo)) {
                return productos[i];
            }

        }

        return null;
    }
                 public static boolean eliminarProducto(String codigo){

                   for(int i = 0; i < contador; i++){

                    if(productos[i].getCodigo().equals(codigo)){

                   for(int j = i; j < contador - 1; j++){
                     productos[j] = productos[j + 1];
            } 

                       contador--;
                      return true;
                    }

                 }

    return false;
}
                  public static boolean editarProducto(String codigo, String nombre, String categoria, double precio, int stock){

                       for(int i = 0; i < contador; i++){

                       if(productos[i].getCodigo().equals(codigo)){

                         productos[i].setNombre(nombre);
                         productos[i].setCategoria(categoria);
                         productos[i].setPrecio(precio);
                         productos[i].setStock(stock);

                       return true;
        }

    }

    return false;
}
                 
                 public static boolean registrarVenta(String codigo, int cantidad){

                  for(int i = 0; i < contador; i++){

                        if(productos[i].getCodigo().equals(codigo)){

                           if(productos[i].getStock() >= cantidad){

                          double total = productos[i].getPrecio() * cantidad;

                         productos[i].setStock(productos[i].getStock() - cantidad);

                         String fechaHora = java.time.LocalDateTime.now().toString();

                        ventas[contadorVentas] = new Venta(codigo, cantidad, total, fechaHora);
                        contadorVentas++;
                return true;
            }

        }
                       
    }

    return false;
}
                 public static void generarReporte(){
                      ReporteController.generarReporteStock(productos, contador);
                 }
                     public static String buscarPorCategoria(String categoria) {

                String resultado = "";

                     for (int i = 0; i < contador; i++) {

                    if (productos[i].getCategoria().equalsIgnoreCase(categoria)) {

                      resultado += "Codigo: " + productos[i].getCodigo() +
                    " | Nombre: " + productos[i].getNombre() +
                    " | Precio: " + productos[i].getPrecio() +
                    " | Stock: " + productos[i].getStock() + "\n";

             }

        }

                if (resultado.equals("")) {
                  return "No se encontraron productos en esa categoria";
               }

             return resultado;
             }
}