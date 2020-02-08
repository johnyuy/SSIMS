package com.sg.ssimsapp;

import androidx.appcompat.app.AppCompatActivity;

import android.app.AlertDialog;
import android.os.Bundle;
import android.view.View;
import android.widget.*;

import com.sg.ssimsapp.Model.UserLogin;
import com.sg.ssimsapp.Remote.ISSIMSApi;
import com.sg.ssimsapp.Remote.RetrofitClient;

import dmax.dialog.SpotsDialog;
import io.reactivex.Scheduler;
import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.functions.Consumer;
import io.reactivex.schedulers.Schedulers;
import retrofit2.Retrofit;

public class MainActivity extends AppCompatActivity {

    ISSIMSApi issimsApi;
    CompositeDisposable compositeDisposable = new CompositeDisposable();

    EditText edt_username, edt_password;
    TextView txt_account;
    Button btn_login;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        //
        issimsApi = RetrofitClient.getInstance().create(ISSIMSApi.class);

        //From layout
        edt_username = (EditText)findViewById(R.id.edt_username);
        edt_password = (EditText)findViewById(R.id.edt_password);
        btn_login = (Button)findViewById(R.id.btn_login);

        btn_login.setOnClickListener(new GridView.OnClickListener() {
            @Override
            public void onClick(View view ){
                final AlertDialog dialog = new SpotsDialog.Builder()
                        .setContext(MainActivity.this)
                        .build();
                dialog.show();

                UserLogin userLogin = new UserLogin(edt_username.getText().toString(), edt_password.getText().toString());

                compositeDisposable.add(issimsApi.Authenticate(userLogin)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(new Consumer<String>() {
                    @Override
                    public void accept(String s) throws Exception {
                        Toast.makeText(MainActivity.this, s, Toast.LENGTH_SHORT).show();
                        dialog.dismiss();
                    }
                    }, new Consumer<Throwable>(){
                        @Override
                        public void accept(Throwable throwable) throws Exception{
                                dialog.dismiss();
                                Toast.makeText(MainActivity.this, throwable.getMessage(),Toast.LENGTH_SHORT).show();

                    }
                }));
            }
        });

    }

    @Override
    protected void onStop(){
        compositeDisposable.clear();
        super.onStop();

    }
}
