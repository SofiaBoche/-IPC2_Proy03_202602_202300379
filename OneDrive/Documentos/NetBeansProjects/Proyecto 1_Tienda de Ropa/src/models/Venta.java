/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package models;

/**
 *
 * @author tofab
 */
public class Venta {
               private String codigoProducto;
               private int cantidad;
               private double total;
               private String fechaHora; 
               
    public Venta(String codigoProducto, int cantidad, double total,String fechaHora) {
        this.codigoProducto = codigoProducto;
        this.cantidad = cantidad;
        this.total = total;
        this.fechaHora = fechaHora;
    }

    public String getCodigoProducto() {
        return codigoProducto;
    }

    public int getCantidad() {
        return cantidad;
    }

    public double getTotal() {
        return total;
    }
    public String getFechaHora() {
    return fechaHora;
    }
}
