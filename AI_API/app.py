from flask import Flask, request, jsonify
import joblib
import numpy as np
import pandas as pd

app = Flask(__name__)

# Chargement des artefacts une fois au démarrage
model = joblib.load('model_irrigation.joblib')
scaler = joblib.load('scaler.joblib')

# Configuration des features dans le bon ordre
FEATURES = ['temperature_sol', 'temperature_air', 'humidite_sol', 'humidite_air', 'luminosite']

# Add this endpoint to your existing app.py
@app.route('/stream', methods=['POST'])
def stream_data():
    try:
        data = request.json
        if not all(feature in data for feature in FEATURES):
            return jsonify({"error": "Missing sensor data"}), 400
        
        # Process the data  
        input_data = [data[f] for f in FEATURES]
        input_array = np.array(input_data).reshape(1, -1)
        input_scaled = scaler.transform(input_array)
        prediction = model.predict(input_scaled)[0]
        
        recommendations = {
            0: "No irrigation needed",
            1: "Light irrigation recommended",
            2: "Moderate irrigation recommended",
            3: "Heavy irrigation recommended"
        }
        
        # Log the received data
        print(f"Received sensor data: {data}")
        print(f"Recommendation: {recommendations[prediction]}")
        
        return jsonify({
            "status": "success",
            "recommendation": recommendations[prediction]
            # "timestamp": datetime.now().isoformat()
        })
        
    except Exception as e:
        return jsonify({"error": str(e)}), 500

@app.route('/')
def home():
    return "API de recommandation d'irrigation - POST /predict avec les données des capteurs"
 
if __name__ == '__main__':
    app.run(port=5001, debug=True) 