import { useMemo } from "react";
import { GoogleMap as GM, useJsApiLoader } from "@react-google-maps/api";

type Props = {
  center?: google.maps.LatLngLiteral;
  zoom?: number;
  className?: string;
};

export default function GoogleMap({
  center = { lat: 56.26392, lng: 9.501785 }, 
  zoom = 7, 
  className = "map",
}: Props) {
  const apiKey = import.meta.env.VITE_GOOGLE_MAPS_API_KEY as string;

  const { isLoaded, loadError } = useJsApiLoader({
    googleMapsApiKey: apiKey,
  });

  const options = useMemo<google.maps.MapOptions>(() => ({}), []);

  if (loadError) {
    return <div style={{ padding: '20px', color: 'red' }}>Fejl ved indlæsning af Google Maps: {loadError.message}</div>;
  }

  if (!isLoaded) {
    return <div style={{ padding: '20px' }}>Loading Google Maps...</div>;
  }

  return (
    <div style={{ 
      height: 'calc(100vh - 60px)', 
      width: '100%'
    }}>
      <GM
        center={center}
        zoom={zoom}
        options={options}
        mapContainerStyle={{
          height: '100%',
          width: '100%'
        }}
        onLoad={(map) => {
          console.log('Map indlæst:', map);
        }}
      />
    </div>
  );
}