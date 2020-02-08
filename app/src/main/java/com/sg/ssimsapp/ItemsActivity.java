package com.sg.ssimsapp;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.os.Bundle;
import android.widget.*;

import com.sg.ssimsapp.Adapter.ItemAdapter;
import com.sg.ssimsapp.Model.Item;
import com.sg.ssimsapp.Remote.ISSIMSApi;
import com.sg.ssimsapp.Remote.RetrofitClient;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class ItemsActivity extends AppCompatActivity {

    private static final String TAG = "ItemsActivity";

    ISSIMSApi issimsApi;
    private RecyclerView recyclerView;
    private ItemAdapter itemAdapter;
    private RecyclerView.LayoutManager layoutManager;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_items);

        //Init RecycleView
        recyclerView = (RecyclerView) findViewById(R.id.recycler_list_item);
        recyclerView.setHasFixedSize(true);
        layoutManager = new LinearLayoutManager(this);
        recyclerView.setLayoutManager(layoutManager);

        //GET Items with API
        issimsApi = RetrofitClient.getInstance().create(ISSIMSApi.class);
        Call<ArrayList<Item>> call = issimsApi.GetAllItems();

        call.enqueue(new Callback<ArrayList<Item>>() {
            @Override
            public void onResponse(Call<ArrayList<Item>> call, Response<ArrayList<Item>> response) {
                //Retrieved items
                ArrayList<Item> items = response.body();
                itemAdapter = new ItemAdapter(ItemsActivity.this, items);
                //Display retrieved items
                recyclerView.setAdapter(itemAdapter);
                Toast.makeText(ItemsActivity.this, "Items Loaded", Toast.LENGTH_SHORT).show();

            }

            @Override
            public void onFailure(Call<ArrayList<Item>> call, Throwable t) {
                Toast.makeText(ItemsActivity.this, "Failed", Toast.LENGTH_SHORT).show();

            }
        });
    }
}
