package com.sg.ssimsapp.Remote;

import com.sg.ssimsapp.Model.*;

import java.util.ArrayList;
import java.util.List;

import io.reactivex.Observable;
import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.POST;

public interface ISSIMSApi {

    //https://localhost:44386/api/LoginApi
    @POST("api/loginApi")
    Observable<String> Authenticate(@Body UserLogin userLogin);

    @GET("api/itemsApi")
    Call<ArrayList<Item>> GetAllItems();

    @GET("api/itemsApi")
    Call<List<Item>> GetItems(@Body String searchString);



    //@POST("")
}
