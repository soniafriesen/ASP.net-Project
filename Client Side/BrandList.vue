<template>   
    <v-container fluid>     
        <v-row justify="center" class="text-center display-2" style="margin-top:.5em;">Brands</v-row>     
        <v-row justify="center">       
            <v-col class="title text-center" style="color:red">{{status}}</v-col>     
        </v-row>     
        <v-row justify="center">       
            <v-col class="text-left display-1">         
                <v-select :items="brands" 
                item-text="name" 
                style="max-height: 50%;"
                item-value="id"
                @input="changeBrand"     
                v-model="selectedid" >
                </v-select>       
            </v-col>     
        </v-row> 
        <div v-if="productitems.length > 0">       
          <v-row justify="center" class="text-center headline">Products in Brand</v-row>       
          <v-row justify="center" style="margin-top:1vh;">         
              <v-col class="text-left display-2">           
                  <v-list style="max-height: 60vh;" class="overflow-y-auto">             
                      <v-list-item-group>               
                            <v-list-item
                            @click="popDialog(item)"  
                            v-for="(item, i) in productitems" 
                            :key="i" 
                            style="border:solid;"
                            >                
                              <v-col style="width:25%;">                   
                                  <v-img 
                                    :src="require('../assets/'+ item.graphicName)"      
                                    class="my-3"                     
                                    style="height:10vh;width:10vh;"                     
                                    aspect-ratio="1"                   
                                    />                 
                                </v-col>                 
                                <v-col style="width:75%;">                   
                                    <v-list-item-content>                     
                                        <v-list-item-title class="title" v-text="item.productName">></v-list-item-title>                   
                                    </v-list-item-content>                 
                                </v-col>               
                            </v-list-item>             
                        </v-list-item-group>           
                    </v-list>         
                </v-col>       
            </v-row>  
            <v-dialog v-model="dialog" v-if="selectProductItem" justify="center" align="center">         
                <v-card>           
                    <v-row>             
                        <v-spacer></v-spacer>             
                        <v-btn text @click="dialog = false" style="font-size:XX-large;margin:1vw;">X</v-btn>           
                    </v-row> 
                    <v-row justify="center" class="title">Computer Information</v-row> 
                    <div>
                        <v-row >  
                            <v-col style="margin:2vw">{{selectProductItem.productName}}</v-col>  
                            <v-col>                  
                               <v-img  
                                    v-if="selectProductItem.graphicName"
                                    :src="require(`../assets/${selectProductItem.graphicName}`)"               
                                    height="15vh"               
                                    width="15vh"               
                                    contain               
                                    aspect-ratio="1"             
                                />  
                            </v-col>
                        </v-row>
                        <v-row justify="center" style="font-size:23px" > MRSP: {{selectProductItem.msrp | currency}}
                        </v-row>
                        <v-row style="margin:2vw; font-size:18px">Description:
                        </v-row> 
                        <v-row style="margin:2vw">{{selectProductItem.description}}</v-row>                                      
                    </div>
                    <v-row style="margin-left:3vw;">             
                        <v-col>Qty:</v-col>             
                        <v-col>               
                            <input                 
                            type="number"                
                            maxlength="3"                 
                            placeholder="enter qty"                 
                            size="3"                 
                            style="width: 15vw;border-bottom:solid;text-align:right"              
                            v-model="qty"               
                            />             
                        </v-col>             
                        <v-col cols="7"></v-col>           
                    </v-row>
                    <v-row justify="center" align="center" style="margin-bottom:2vh;margin-left:3vw;">        
                        <v-col>           
                            <v-btn medium outlined color="default" @click="addTocart">Add To cart</v-btn>         
                        </v-col>         
                        <v-col>           
                            <v-btn medium outlined color="default" @click="viewCart">View cart</v-btn>         
                        </v-col>       
                    </v-row>           
                    <v-row justify="center" align="center" style="padding-bottom:5vh;">{{this.dialogStatus}}</v-row>                
                </v-card>      
            </v-dialog>     
        </div>          
     </v-container> 
</template>
<script> 

import fetcher from "../mixins/fetcher"; 
export default {   
    
    name: "BrandsList",   
    data() {     
        return {       
            brands: [],       
            status: {},
            selectedid: 0,       
            productitems: [],
            dialog: false,
            selectProductItem: {},
            qty: 0,       
            cart: [],       
            dialogStatus: ""      
        };   
    },   
    mixins: [fetcher], 
    mounted: async function() {     // don't use arrow function here     
        try 
        {       
            this.status = "fetching brands from server...";                 
            this.brands = await this.$_getdata("brand"); // $_getdata in mixin        
            this.status = `loaded ${this.brands.length + 1} brands`;       
            this.brands.unshift({ name: "Current Brands", id: 0 });     
        } 
        catch (err) 
        {       
            console.log(err);       
            this.status = `Error has occured: ${err.message}`;     
        }   
    }, 
    methods: {     
        changeBrand: async function(brandid) {       
            if (brandid > 0) {   // don't use arrow function here         
                try {           
                    this.status = `fetching items for ${brandid}...`;           
                    this.productitems = await this.$_getdata(`productitem/${brandid}`);           
                    this.status = `found ${this.productitems.length} items`;         
                }
                catch (err) {           
                    console.log(err);           
                    this.status = `Error has occured: ${err.message}`;         
                }       
            }     
        },
        popDialog: function(item) {      
            this.dialogStatus = "";       
            this.dialog = !this.dialog;       
            this.selectProductItem = item;     
        },
        addTocart: function() {       
                const index = this.cart.findIndex(  
                // is item already on the cart         
                item => item.id === this.selectProductItem.id
            );
            if (this.qty !== "0") {         
                index === -1           
                ? this.cart.push({               
                    qty: parseInt(this.qty),               
                    item: this.selectProductItem      
                }) // add           
                : (this.cart[index] = {  
                // replace               
                qty: parseInt(this.qty),               
                item: this.selectProductItem             
                }); 
            this.dialogStatus = `${this.qty} products(s) added`;       
            } else {         
                index === -1 ? null : this.cart.splice(index, 1); 
                // remove         
                this.dialogStatus = `products(s) removed`;       
            }       
            sessionStorage.setItem("cart", JSON.stringify(this.cart));     
        },
        viewCart: function() {   
            this.$router.push({     
                name: "cart"   
            }); 
        }    
    }
};
</script>