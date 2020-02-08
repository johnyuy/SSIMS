package com.sg.ssimsapp.Adapter;

import android.content.Context;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ListAdapter;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.recyclerview.widget.RecyclerView;

import com.sg.ssimsapp.Model.Item;
import com.sg.ssimsapp.R;

import org.w3c.dom.Text;

import java.util.ArrayList;
import java.util.List;

public class ItemAdapter extends RecyclerView.Adapter<ItemAdapter.ItemViewHolder> {

    private static final String TAG = "ItemAdapter";

    private ArrayList<Item> items = new ArrayList<>();
    private Context context;

    public ItemAdapter(Context mContext, ArrayList<Item> items ) {
        this.items = items;
        this.context = mContext;
    }

    @Override
    public ItemViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {

        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.layout_listitem, parent, false);
        ItemViewHolder holder = new ItemViewHolder(view);
        return holder;
    }

    public static class ItemViewHolder extends RecyclerView.ViewHolder{

        TextView tvID;
        TextView tvCategory;
        TextView tvDescription;
        TextView tvUnitOfMeasure;
        RelativeLayout parentLayout;

        public ItemViewHolder(View itemView) {
            super(itemView);
            this.tvID = itemView.findViewById(R.id.tv_id);
            this.tvCategory = itemView.findViewById(R.id.tv_category);
            this.tvDescription = itemView.findViewById(R.id.tv_description);
            this.tvUnitOfMeasure = itemView.findViewById(R.id.tv_uom);
        }
    }

    @Override
    public void onBindViewHolder(@NonNull ItemViewHolder holder, final int position) {
        Log.d(TAG,"onBindViewHolder: called." );

        holder.tvID.setText(items.get(position).getID());
        holder.tvCategory.setText(items.get(position).getCategory());
        holder.tvDescription.setText(items.get(position).getDescription());
        holder.tvUnitOfMeasure.setText(items.get(position).getUnitOfMeasure());

        holder.parentLayout.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Log.d(TAG, "onCLick: clicked on: " + items.get(position).getDescription());

                Toast.makeText(context, items.get(position).getDescription(), Toast.LENGTH_SHORT).show();
            }
        });
    }

    @Override
    public int getItemCount() {
        return items.size();
    }

}
