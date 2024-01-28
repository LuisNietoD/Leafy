using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScriptableCard))]
public class ScriptableCardEditor : Editor
{
    SerializedProperty nameProp;
    SerializedProperty IDProp;
    SerializedProperty artworkProp;
    SerializedProperty backgroundProp;
    SerializedProperty backgroundColorProp;
    SerializedProperty typeProp;

    // Bool section
    SerializedProperty sellableProp;
    SerializedProperty craftableProp;
    SerializedProperty harvestProp;

    // Section
    SerializedProperty priceProp;
    SerializedProperty lifeProp;
    SerializedProperty infiniteProp;
    SerializedProperty timeToCraftProp;

    void OnEnable()
    {
        // Initialize serialized properties
        nameProp = serializedObject.FindProperty("name");
        IDProp = serializedObject.FindProperty("ID");
        artworkProp = serializedObject.FindProperty("artwork");
        backgroundProp = serializedObject.FindProperty("background");
        backgroundColorProp = serializedObject.FindProperty("background_color");
        typeProp = serializedObject.FindProperty("type");

        sellableProp = serializedObject.FindProperty("sellable");
        craftableProp = serializedObject.FindProperty("craftable");
        harvestProp = serializedObject.FindProperty("harvestable");

        priceProp = serializedObject.FindProperty("price");
        lifeProp = serializedObject.FindProperty("life");
        infiniteProp = serializedObject.FindProperty("infinite");
        timeToCraftProp = serializedObject.FindProperty("timeToCraft");
    }

    public override void OnInspectorGUI()
    {
        // Update the serialized object
        serializedObject.Update();

        // Display the standard ScriptableObject fields
        EditorGUILayout.PropertyField(nameProp);
        EditorGUILayout.PropertyField(IDProp);
        EditorGUILayout.PropertyField(artworkProp);
        EditorGUILayout.PropertyField(backgroundProp);
        EditorGUILayout.PropertyField(backgroundColorProp);
        EditorGUILayout.PropertyField(typeProp);

        EditorGUILayout.PropertyField(sellableProp);
        EditorGUILayout.PropertyField(craftableProp);
        EditorGUILayout.PropertyField(harvestProp);

        // Display sell information
        if (sellableProp.boolValue)
        {
            EditorGUILayout.PropertyField(priceProp, new GUIContent("Price"));
        }

        // Display craft information
        if (craftableProp.boolValue)
        {
            EditorGUILayout.PropertyField(timeToCraftProp, new GUIContent("Time to Craft"));
        }

        // Check if infinite is true, then show life
        if (harvestProp.boolValue)
        {
            EditorGUILayout.PropertyField(infiniteProp, new GUIContent("Infinite"));
            if (!infiniteProp.boolValue)
                EditorGUILayout.PropertyField(lifeProp, new GUIContent("Life"));
        }

        // Custom preview area
        EditorGUILayout.Space();

        // Combine name and sprite for preview
        string cardPreviewText = nameProp.stringValue;
        Sprite cardPreviewSprite = artworkProp.objectReferenceValue as Sprite;

        if (cardPreviewSprite != null)
        {
            // Display the preview with name and sprite
            GUILayout.Label("Card Preview", EditorStyles.boldLabel);

            // Get the icon texture
            Texture2D iconTexture = AssetPreview.GetAssetPreview(artworkProp.objectReferenceValue);

            // Get the background texture
            Texture2D backgroundTexture = AssetPreview.GetAssetPreview(backgroundProp.objectReferenceValue);

            // Calculate position for centering
            float centerX = (EditorGUILayout.GetControlRect(GUILayout.Width(200), GUILayout.Height(300)).width - 200) * 0.5f;
            Rect centeredRect = GUILayoutUtility.GetLastRect();
            centeredRect.x += centerX;

            // Create a new texture with the blended background color
            Texture2D finalBackground = GetBlendedBackground(backgroundTexture, backgroundColorProp.colorValue);

            // Draw the final blended background
            GUI.DrawTexture(centeredRect, finalBackground);

            // Calculate a scaled-down size for the icon (assuming the icon is square)
            float scaleDownFactor = 0.5f;
            float scaledIconSize = Mathf.Min(200, 300) * scaleDownFactor;

            // Calculate centered position for the scaled icon
            float iconX = centeredRect.x + (centeredRect.width - scaledIconSize) * 0.5f;
            float iconY = centeredRect.y + (centeredRect.height - scaledIconSize) * 0.5f;

            // Draw scaled-down icon on top of the background
            Rect iconRect = new Rect(iconX, iconY, scaledIconSize, scaledIconSize);
            GUI.DrawTexture(iconRect, iconTexture);

            // Display text on the texture using Handles.Label
            string labelText = nameProp.stringValue;
            GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
            labelStyle.normal.textColor = Color.black;
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.fontSize = 16; // Increased font size for more boldness

            Vector2 labelSize = labelStyle.CalcSize(new GUIContent(labelText));
            float labelX = centeredRect.x + (centeredRect.width - labelSize.x) * 0.5f;
            float labelY = centeredRect.y + centeredRect.height * 0.22f; // Centered at 5% of the height from the top
            Vector3 labelPosition = new Vector3(labelX, labelY, 0f);

            Handles.Label(labelPosition, labelText, labelStyle);
        }

        // Apply changes to the serialized object
        serializedObject.ApplyModifiedProperties();
    }
    
    private Texture2D GetBlendedBackground(Texture2D backgroundTexture, Color backgroundColor)
    {
        int width = backgroundTexture.width;
        int height = backgroundTexture.height;

        // Get the pixels of the background texture
        Color[] backgroundPixels = backgroundTexture.GetPixels();

        // Create a new array for the final pixels
        Color[] finalPixels = new Color[width * height];

        for (int i = 0; i < finalPixels.Length; i++)
        {
            // Check for almost white in the background
            if (backgroundPixels[i].r >= 0.9f && backgroundPixels[i].a > 0.2f)
            {
                // Blend with the specified backgroundColor
                finalPixels[i] = backgroundColor;
            }
            else if (backgroundPixels[i].a > 0.2f)
            {
                finalPixels[i] = backgroundPixels[i];
            }
        }

        // Create a new texture with the blended pixels
        Texture2D finalBackground = new Texture2D(width, height);
        finalBackground.SetPixels(finalPixels);
        finalBackground.Apply();
        return finalBackground;
    }
    
}
