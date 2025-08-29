import { useMemo } from "react";
import { GoogleMap as GM, LoadScript } from "@react-google-maps/api";

type Props = {
  center?: google.maps.LatLngLiteral;
  zoom?: number;
  className?: string; // controls size via CSS
};

export default function GoogleMap({
  center = { lat: 55.6761, lng: 12.5683 },
  zoom = 12,
  className = "map",
}: Props) {
  const options = useMemo<google.maps.MapOptions>(() => ({}), []);

  return (
    <LoadScript googleMapsApiKey={import.meta.env.VITE_GOOGLE_MAPS_API_KEY as string}>
      <GM
        center={center}
        zoom={zoom}
        options={options}
        mapContainerClassName={className}
      />
    </LoadScript>
  );
}
