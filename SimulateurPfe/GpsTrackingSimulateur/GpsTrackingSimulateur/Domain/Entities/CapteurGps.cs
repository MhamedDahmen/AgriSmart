using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpsTrackingSimulateur.Domain.Entities
{
    public class CapteurGps
    {
        private static readonly Random _random = new();
        private double _currentLat;
        private double _currentLng;

        private readonly double _centerLat;
        private readonly double _centerLng;
        private readonly double _maxDistance = 0.001; // ~100m en degrés (valeur max pour rester dans le champ)

        public string TrackableId { get; }
        public string FieldId { get; }

        public CapteurGps(string trackableId, string fieldId, double initialLat, double initialLng)
        {
            TrackableId = trackableId;
            FieldId = fieldId;
            _centerLat = initialLat;
            _centerLng = initialLng;
            _currentLat = initialLat;
            _currentLng = initialLng;
        }

        public (double latitude, double longitude) GenerateValue()
        {
            bool isAnomalie = _random.NextDouble() < 0.05; // 5% de chance de générer une anomalie
            double delta = isAnomalie ? 0.005 : 0.0001;     // 0.005° ≈ 500m

            double newLat, newLng;

            do
            {
                newLat = _currentLat + (_random.NextDouble() * 2 - 1) * delta;
                newLng = _currentLng + (_random.NextDouble() * 2 - 1) * delta;
            }
            while (!IsWithinBounds(newLat, newLng) && !isAnomalie); // garder dans le champ si pas une anomalie

            _currentLat = newLat;
            _currentLng = newLng;

            return (_currentLat, _currentLng);
        }

        private bool IsWithinBounds(double lat, double lng)
        {
            double latDiff = Math.Abs(lat - _centerLat);
            double lngDiff = Math.Abs(lng - _centerLng);
            return latDiff <= _maxDistance && lngDiff <= _maxDistance;
        }
    }
}
