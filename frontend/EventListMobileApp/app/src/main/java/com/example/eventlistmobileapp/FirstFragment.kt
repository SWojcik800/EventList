package com.example.eventlistmobileapp

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.LinearLayout
import android.widget.Toast
import androidx.navigation.Navigation
import androidx.navigation.fragment.findNavController
import com.example.eventlistmobileapp.Commons.ApiService
import com.example.eventlistmobileapp.Commons.AppConsts
import com.example.eventlistmobileapp.Commons.Helpers.DateFormatter
import com.example.eventlistmobileapp.UI.CardComponent
import com.example.eventlistmobileapp.UI.CardComponentItem
import com.example.eventlistmobileapp.databinding.ActivityMainBinding
import com.example.eventlistmobileapp.databinding.FragmentFirstBinding
import com.google.android.material.snackbar.Snackbar
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

/**
 * A simple [Fragment] subclass as the default destination in the navigation.
 */
class FirstFragment : Fragment() {

    private var _binding: FragmentFirstBinding? = null

    // This property is only valid between onCreateView and
    // onDestroyView.
    private val binding get() = _binding!!

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {

        _binding = FragmentFirstBinding.inflate(inflater, container, false)
        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        val activity = requireActivity()
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
        val containerWrapper = view?.findViewById<LinearLayout>(R.id.cardContainerWrapper)
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

        val containerWrapper = view?.findViewById<LinearLayout>(R.id.cardContainerWrapper)
        items?.forEach { item ->
            val cardComponent: CardComponent = CardComponent(requireContext())

            val cardListItems = listOf<CardComponentItem>(
                CardComponentItem("From", DateFormatter.formatDate(item.startTime)),
                CardComponentItem("To", DateFormatter.formatDate(item.endTime)),
                CardComponentItem("Location", item.location.street)
            )

            cardComponent.setPropertiesFrom(cardListItems)
                .setTitle("${item.name} - ${item.lecturerNames.joinToString(",")}")

            // Set click listener
            cardComponent.setCardOnClickListener {
                Toast.makeText(requireContext(), "${item.name} lecture clicked", Toast.LENGTH_SHORT).show()
                // Navigate to lecture details fragment
                Navigation.findNavController(requireActivity(), R.id.nav_host_fragment_content_main)
                    .navigate(R.id.action_FirstFragment_to_SecondFragment)
            }
            containerWrapper?.addView(cardComponent)
        }

    }
}