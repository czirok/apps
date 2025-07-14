export default (context) => {
	return {
		map: {
			inline: false,
			annotation: false,
			sourcesContent: false,
		},
		plugins: {
			autoprefixer: {
				cascade: false,
			},
			rtlcss: context.env === 'RTL',
			cssnano: {},
		},
	}
}
