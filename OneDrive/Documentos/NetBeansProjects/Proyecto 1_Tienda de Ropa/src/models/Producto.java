/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package models;

/**
 *
 * @author tofab
 */
public class Producto {
    
                     private String codigo;
                     private String nombre;
                     private String categoria;
                     private double precio;
                     private int stock;

    public Producto(String codigo, String nombre, String categoria, double precio, int stock) {
        this.codigo = codigo;
        this.nombre = nombre;
        this.categoria = categoria;
        this.precio = precio;
        this.stock = stock;
    }

    public String getCodigo() {
        return codigo;
    }

    public String getNombre() {
        return nombre;
    }

    public String getCategoria() {
        return categoria;
    }

    public double getPrecio() {
        return precio;
    }

    public int getStock() {
        return stock;
    }
    
    public void setNombre(String nombre){
        this.nombre = nombre;
    }

    public void setCategoria(String categoria){
        this.categoria = categoria;
    }

    public void setPrecio(double precio){
        this.precio = precio;
    }

    public void setStock(int stock) {
        this.stock = stock;
    }
}
