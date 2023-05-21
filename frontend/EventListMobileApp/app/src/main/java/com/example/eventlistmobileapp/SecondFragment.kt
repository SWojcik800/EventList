package com.example.eventlistmobileapp

import android.content.Intent
import android.os.Bundle
import android.preference.PreferenceManager.OnActivityResultListener
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.LinearLayout
import android.widget.Toast
import androidx.activity.result.contract.ActivityResultContracts
import androidx.navigation.Navigation
import androidx.navigation.fragment.findNavController
import com.example.eventlistmobileapp.Commons.ApiService
import com.example.eventlistmobileapp.Commons.AppConsts
import com.example.eventlistmobileapp.Commons.Helpers.DateFormatter
import com.example.eventlistmobileapp.UI.CardComponent
import com.example.eventlistmobileapp.UI.CardComponentItem
import com.example.eventlistmobileapp.databinding.FragmentSecondBinding
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

/**
 * A simple [Fragment] subclass as the second destination in the navigation.
 */
class SecondFragment : Fragment() {

    private var _binding: FragmentSecondBinding? = null

    // This property is only valid between onCreateView and
    // onDestroyView.
    private val binding get() = _binding!!
    private lateinit var _currentActivity: MainActivity

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {

        _binding = FragmentSecondBinding.inflate(inflater, container, false)

        return binding.root

    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)


        _currentActivity = requireActivity() as MainActivity
    }

    override fun onResume() {
        super.onResume()
        refreshCards()
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }

    private fun refreshCards() {
        val containerWrapper = view?.findViewById<LinearLayout>(R.id.eventCardContainerWrapper)
        containerWrapper?.removeAllViews()
        loadCards()
    }

    private fun loadCards() {

        val retrofit = Retrofit.Builder()
            .baseUrl(AppConsts.apiBaseUrl) // Replace with your API base URL
            .addConverterFactory(GsonConverterFactory.create())
            .build()
        val apiService = retrofit.create(ApiService::class.java)
        val items = apiService.getLectures().execute().body()?.items

        val containerWrapper = view?.findViewById<LinearLayout>(R.id.eventCardContainerWrapper)
        items?.forEach { item ->
            val cardComponent: CardComponent = CardComponent(requireContext())

            val cardListItems = listOf<CardComponentItem>(
                CardComponentItem("From", DateFormatter.formatDate(item.startTime)),
                CardComponentItem("To", DateFormatter.formatDate(item.endTime)),
                CardComponentItem("Location", item.location.street)
            )

            cardComponent.setPropertiesFrom(cardListItems)
                .setTitle("${item.name} - ${item.lecturerNames.joinToString(",")}")
            containerWrapper?.addView(cardComponent)
        }
    }
}